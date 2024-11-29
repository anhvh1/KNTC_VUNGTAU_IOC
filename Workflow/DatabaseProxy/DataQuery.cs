using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workflow.Model;
using System.Data;
//using Com.Gosol.CMS.Utility;
using System.Data.SqlClient;
using Com.Gosol.KNTC.Ultilities;

namespace Workflow
{
    public class DataQuery
    {
        #region Declare param

        private const string PARM_DOCUMENTID = "@DocumentID";
        private const string PARM_WORKFLOWID = "@WorkflowID";
        private const string PARM_STATEID = "@StateID";
        private const string PARM_DUEDATE = "@DueDate";

        private const string PARM_WORKFLOWCODE = "@WorkflowCode";
        private const string PARM_WORKFLOWNAME = "@WorkflowName";

        private const string PARM_STATE = "@StateID";
        private const string PARM_STATE_NAME = "@StateName";
        private const string PARM_CURRENT_STATE_ID = "@CurrentStateID";
        private const string PARM_NEXT_STATE_ID = "@NextStateID";
        private const string PARM_CURRENT_STATE_NAME = "@CurrentStateName";
        private const string PARM_NEXT_STATE_NAME = "@NextStateName";

        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_END_DATE = "@EndDate";

        private const string PARM_COMMAND_CODE = "@CommandCode";

        private const string PARM_TRANSITION_ID = "@TransitionID";
        private const string PARM_COMMENT = "@Comment";
        private const string PARM_USER_ID = "@UserID";
        private const string PARM_MODIFIED_DATE = "@ModifiedDate";

        #endregion

        #region Document query

        private const string INSERT_DOCUMENT = "insert into Document (DocumentID, WorkflowID, StateID, DueDate) OUTPUT INSERTED.DocumentID values (@DocumentID, @WorkflowID, @StateID, @DueDate)";
        private const string UPDATE_DOCUMENT = "update Document set StateID = @StateID, DueDate = @DueDate where DocumentID = @DocumentID";
        private const string DELETE_DOCUMENT = "delete from Document where DocumentID = @DocumentID;delete from TransitionHistory where DocumentID = @DocumentID;";

        private const string GET_DOCUMENT_BY_ID = "select * from Document where DocumentID = @DocumentID";
        private const string GET_PREVSTATE_OF_DOCUMENT = "select StateName from [State] where StateID in (select case when IsNext='True' then CurrentStateID else NextStateID end  from Transition where TransitionID in( select top 1 TransitionID from TransitionHistory where DocumentID=@DocumentID order by TransitionHistoryID desc))";
        private const string GET_CURRENT_STATE_OF_DOCUMENT = "select StateName from Document a join State b on a.StateID = b.StateID where DocumentID = @DocumentID";
        private const string GET_DOCUMENT_BY_CURRENT_STATE_AND_NEXT_STATE = "select DocumentID from Document a join TransitionHistory b on a.DocumentID = b.DocumentID join Transition c on b.TransitionID = c.TransitionID join State d on b.CurrentStateID = d.StateID join State e on b.NextStateID = e.StateID where d.StateName = @CurrentStateName and e.StateName = @NextStateName and (a.DueDate >= @StartDate or @StartDate is null) and (a.DueDate <= @EndDate or @EndDate is null) group by a.DocumentID";
        private const string GET_DOCUMENT_BY_STATE = "select a.* from Document a join State b on a.StateID = b.StateID where StateName = @StateName  and (DueDate >= @StartDate or @StartDate is null) and (DueDate <= @EndDate or @EndDate is null)";

        #endregion

        #region Workflow query

        private const string GET_WORKFLOW_ID_BY_CODE = "select WorkflowID from Workflow where WorkflowCode = @WorkflowCode";
        private const string GET_FIRST_STATE_OF_WF = "select StateID from State a left join Workflow b on a.WorkflowID = b.WorkflowID where b.WorkflowCode = @WorkflowCode order by StateOrder";
        #endregion

        #region Transtion query

        private const string GET_NEXT_STATE = "select NextStateID from Transition a join Command b on a.CommandID = b.CommandID where CurrentStateID = @CurrentStateID and b.CommandCode = @CommandCode";
        private const string GET_TRANSTION_BY_STATE_AND_COMMAND = "select a.* from Transition a join Command b on a.CommandID = b.CommandID where CurrentStateID = @CurrentStateID and CommandCode = @CommandCode";
        private const string GET_COMMAND_BY_STATE = "select a.CommandID, b.CommandCode, b.CommandName from Transition a join Command b on a.CommandID = b.CommandID where CurrentStateID = @CurrentStateID order by b.ViewOrder";
        private const string INSERT_TRANSTION_HISTORY = "insert into TransitionHistory ([DocumentID], [TransitionID], [DueDate], [Comment],[UserID],[ModifiedDate]) OUTPUT INSERTED.TransitionHistoryID values (@DocumentID, @TransitionID, @DueDate, @Comment, @UserID, @ModifiedDate)";

        #endregion

        public int GetWorkflowIDByCode(string workflowCode)
        {
            object wfID;

            IDbConnection dbConnection = DatabaseProxy.CreateDBConnection();
            IDbCommand dbCommand = DatabaseProxy.CreateDBCommand(dbConnection, GET_WORKFLOW_ID_BY_CODE);
            DatabaseProxy.AddParameter(dbCommand, PARM_WORKFLOWCODE, workflowCode);

            try
            {
                dbConnection.Open();
                wfID = dbCommand.ExecuteScalar();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                dbConnection.Close();
            }

            return (int)wfID;
        }

        public int GetFirstState(string workflowCode)
        {
            object stateID = 0;

            IDbConnection dbConnection = DatabaseProxy.CreateDBConnection();
            IDbCommand dbCommand = DatabaseProxy.CreateDBCommand(dbConnection, GET_FIRST_STATE_OF_WF);
            DatabaseProxy.AddParameter(dbCommand, PARM_WORKFLOWCODE, workflowCode);

            try
            {
                dbConnection.Open();
                stateID = dbCommand.ExecuteScalar();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                dbConnection.Close();
            }

            return (int)stateID;
        }

        public DocumentModel GetDocumentData(IDataReader dataReader)
        {
            DocumentModel model = new DocumentModel();

            model.DocumentID = Utils.ConvertToInt32(dataReader["DocumentID"], 0);
            model.WorkflowID = Utils.ConvertToInt32(dataReader["WorkflowID"], 0);
            model.StateID = Utils.ConvertToInt32(dataReader["StateID"], 0);
            model.DueDate = Utils.ConvertToDateTime(dataReader["DueDate"], DateTime.MinValue);

            return model;
        }

        public int InsertDocument(DocumentModel docModel)
        {
            object insertID = 0;

            IDbConnection dbConnection = DatabaseProxy.CreateDBConnection();
            IDbCommand dbCommand = DatabaseProxy.CreateDBCommand(dbConnection, INSERT_DOCUMENT);
            DatabaseProxy.AddParameter(dbCommand, PARM_DOCUMENTID, docModel.DocumentID);
            DatabaseProxy.AddParameter(dbCommand, PARM_WORKFLOWID, docModel.WorkflowID);
            DatabaseProxy.AddParameter(dbCommand, PARM_STATEID, docModel.StateID);
            if (docModel.DueDate != null)
            {
                DateTime dueDate = Convert.ToDateTime(docModel.DueDate);
                DatabaseProxy.AddParameter(dbCommand, PARM_DUEDATE, dueDate);
            }
            else
            {
                DatabaseProxy.AddParameter(dbCommand, PARM_DUEDATE, DBNull.Value);
            }

            dbConnection.Open();
            using (SqlTransaction trans = ((SqlConnection)dbConnection).BeginTransaction())
            {
                try
                {
                    dbCommand.Transaction = trans;
                    insertID = dbCommand.ExecuteScalar();

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }


            }
            dbConnection.Close();

            return (int)insertID;
        }

        public void UpdateDocumentState(DocumentModel documentModel)
        {
            IDbConnection dbConnection = DatabaseProxy.CreateDBConnection();
            IDbCommand dbCommand = DatabaseProxy.CreateDBCommand(dbConnection, UPDATE_DOCUMENT);
            DatabaseProxy.AddParameter(dbCommand, PARM_DOCUMENTID, documentModel.DocumentID);
            DatabaseProxy.AddParameter(dbCommand, PARM_STATEID, documentModel.StateID);

            if (documentModel.DueDate != DateTime.MinValue)
            {
                DateTime dueDate = Convert.ToDateTime(documentModel.DueDate);
                DatabaseProxy.AddParameter(dbCommand, PARM_DUEDATE, dueDate);
            }
            else
            {
                DatabaseProxy.AddParameter(dbCommand, PARM_DUEDATE, DBNull.Value);
            }

            try
            {
                dbConnection.Open();
                dbCommand.ExecuteScalar();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public bool DeleteDocument(DocumentModel documentModel)
        {
            bool result = false;
            IDbConnection dbConnection = DatabaseProxy.CreateDBConnection();
            IDbCommand dbCommand = DatabaseProxy.CreateDBCommand(dbConnection, DELETE_DOCUMENT);
            DatabaseProxy.AddParameter(dbCommand, PARM_DOCUMENTID, documentModel.DocumentID);

            try
            {
                dbConnection.Open();
                dbCommand.ExecuteScalar();
                result = true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                dbConnection.Close();
            }
            return result;
        }

        public DocumentModel GetDocumentByID(int documentID)
        {
            DocumentModel model = new DocumentModel();

            IDbConnection dbConnection = DatabaseProxy.CreateDBConnection();
            IDbCommand dbCommand = DatabaseProxy.CreateDBCommand(dbConnection, GET_DOCUMENT_BY_ID);
            DatabaseProxy.AddParameter(dbCommand, PARM_DOCUMENTID, documentID);

            try
            {
                dbConnection.Open();
                IDataReader dataReader = dbCommand.ExecuteReader();
                if (dataReader.Read())
                {
                    model = GetDocumentData(dataReader);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                dbConnection.Close();
            }

            return model;
        }

        public string GetPrevStateOfDocument(int documentID)
        {
            string result = "";

            IDbConnection dbConnection = DatabaseProxy.CreateDBConnection();
            IDbCommand dbCommand = DatabaseProxy.CreateDBCommand(dbConnection, GET_PREVSTATE_OF_DOCUMENT);
            DatabaseProxy.AddParameter(dbCommand, PARM_DOCUMENTID, documentID);

            try
            {
                dbConnection.Open();
                IDataReader dataReader = dbCommand.ExecuteReader();
                if (dataReader.Read())
                {
                    result = Utils.ConvertToString(dataReader["StateName"], string.Empty);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                dbConnection.Close();
            }

            return result;
        }

        public string GetCurrentStateOfDocument(int documentID)
        {
            string result = "";

            IDbConnection dbConnection = DatabaseProxy.CreateDBConnection();
            IDbCommand dbCommand = DatabaseProxy.CreateDBCommand(dbConnection, GET_CURRENT_STATE_OF_DOCUMENT);
            DatabaseProxy.AddParameter(dbCommand, PARM_DOCUMENTID, documentID);

            try
            {
                dbConnection.Open();
                IDataReader dataReader = dbCommand.ExecuteReader();
                if (dataReader.Read())
                {
                    result = Utils.ConvertToString(dataReader["StateName"], string.Empty);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                dbConnection.Close();
            }

            return result;
        }

        public List<int> GetDocumentByPrevState(string currentStateName, string nextStateName, DateTime startDate, DateTime endDate)
        {
            List<int> docIDList = new List<int>();

            IDbConnection dbConnection = DatabaseProxy.CreateDBConnection();
            IDbCommand dbCommand = DatabaseProxy.CreateDBCommand(dbConnection, GET_DOCUMENT_BY_CURRENT_STATE_AND_NEXT_STATE);
            DatabaseProxy.AddParameter(dbCommand, PARM_CURRENT_STATE_NAME, currentStateName);
            DatabaseProxy.AddParameter(dbCommand, PARM_NEXT_STATE_NAME, nextStateName);
            if (startDate != DateTime.MinValue)
                DatabaseProxy.AddParameter(dbCommand, PARM_START_DATE, startDate);
            if (endDate != DateTime.MinValue)
                DatabaseProxy.AddParameter(dbCommand, PARM_END_DATE, endDate);

            try
            {
                dbConnection.Open();
                IDataReader dataReader = dbCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    int docID = Utils.ConvertToInt32(dataReader["DocumentID"], 0);
                    if (docID != 0) docIDList.Add(docID);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                dbConnection.Close();
            }

            return docIDList;
        }

        public List<DocumentModel> GetDocumentByState(string stateName, DateTime startDate, DateTime endDate)
        {
            List<DocumentModel> docList = new List<DocumentModel>();

            IDbConnection dbConnection = DatabaseProxy.CreateDBConnection();
            IDbCommand dbCommand = DatabaseProxy.CreateDBCommand(dbConnection, GET_DOCUMENT_BY_STATE);
            DatabaseProxy.AddParameter(dbCommand, PARM_STATE_NAME, stateName);
            if (startDate != DateTime.MinValue)
                DatabaseProxy.AddParameter(dbCommand, PARM_START_DATE, startDate);
            else DatabaseProxy.AddParameter(dbCommand, PARM_START_DATE, DBNull.Value);
            if (endDate != DateTime.MinValue)
                DatabaseProxy.AddParameter(dbCommand, PARM_END_DATE, endDate);
            else DatabaseProxy.AddParameter(dbCommand, PARM_END_DATE, DBNull.Value);

            try
            {
                dbConnection.Open();
                IDataReader dataReader = dbCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    DocumentModel model = GetDocumentData(dataReader);
                    docList.Add(model);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                dbConnection.Close();
            }

            return docList;
        }

        public int GetNextState(int currentStateID, string commandCode)
        {
            object nextStateID;

            IDbConnection dbConnection = DatabaseProxy.CreateDBConnection();
            IDbCommand dbCommand = DatabaseProxy.CreateDBCommand(dbConnection, GET_NEXT_STATE);
            DatabaseProxy.AddParameter(dbCommand, PARM_CURRENT_STATE_ID, currentStateID);
            DatabaseProxy.AddParameter(dbCommand, PARM_COMMAND_CODE, commandCode);

            try
            {
                dbConnection.Open();
                nextStateID = dbCommand.ExecuteScalar();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                dbConnection.Close();
            }

            return (int)nextStateID;

        }

        public TransitionModel GetTransitionByStateAndCommand(int stateID, string commandCode)
        {
            TransitionModel transModel = new TransitionModel();

            IDbConnection dbConnection = DatabaseProxy.CreateDBConnection();
            IDbCommand dbCommand = DatabaseProxy.CreateDBCommand(dbConnection, GET_TRANSTION_BY_STATE_AND_COMMAND);
            DatabaseProxy.AddParameter(dbCommand, PARM_CURRENT_STATE_ID, stateID);
            DatabaseProxy.AddParameter(dbCommand, PARM_COMMAND_CODE, commandCode);

            try
            {
                dbConnection.Open();
                IDataReader dataReader = dbCommand.ExecuteReader();
                if (dataReader.Read())
                {
                    transModel.TransitionID = Utils.ConvertToInt32(dataReader["TransitionID"], 0);
                    transModel.CurrentStateID = Utils.ConvertToInt32(dataReader["CurrentStateID"], 0);
                    transModel.NextStateID = Utils.ConvertToInt32(dataReader["NextStateID"], 0);
                    transModel.CommandID = Utils.ConvertToInt32(dataReader["CommandID"], 0);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                dbConnection.Close();
            }

            return transModel;
        }

        public List<CommandModel> GetCommandByState(int stateID)
        {
            List<CommandModel> commandList = new List<CommandModel>();

            IDbConnection dbConnection = DatabaseProxy.CreateDBConnection();
            IDbCommand dbCommand = DatabaseProxy.CreateDBCommand(dbConnection, GET_COMMAND_BY_STATE);
            DatabaseProxy.AddParameter(dbCommand, PARM_CURRENT_STATE_ID, stateID);

            try
            {
                dbConnection.Open();
                IDataReader dataReader = dbCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    CommandModel commandModel = new CommandModel();
                    commandModel.CommandID = Utils.ConvertToInt32(dataReader["CommandID"], 0);
                    commandModel.CommandCode = Utils.GetString(dataReader["CommandCode"], string.Empty);
                    commandModel.CommandName = Utils.GetString(dataReader["CommandCode"], string.Empty);
                    commandList.Add(commandModel);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                dbConnection.Close();
            }

            return commandList;
        }

        public int InsertTransitionHistory(TransitionHistoryModel transModel)
        {
            object insertID;

            IDbConnection dbConnection = DatabaseProxy.CreateDBConnection();
            IDbCommand dbCommand = DatabaseProxy.CreateDBCommand(dbConnection, INSERT_TRANSTION_HISTORY);
            DatabaseProxy.AddParameter(dbCommand, PARM_TRANSITION_ID, transModel.TransitionID);
            DatabaseProxy.AddParameter(dbCommand, PARM_DOCUMENTID, transModel.DocumentID);
            if (transModel.DueDate != DateTime.MinValue)
            {
                DatabaseProxy.AddParameter(dbCommand, PARM_DUEDATE, transModel.DueDate);
            }
            else
            {
                DatabaseProxy.AddParameter(dbCommand, PARM_DUEDATE, DBNull.Value);
            }
            DatabaseProxy.AddParameter(dbCommand, PARM_COMMENT, !string.IsNullOrEmpty(transModel.Comment) ? transModel.Comment : string.Empty);
            DatabaseProxy.AddParameter(dbCommand, PARM_USER_ID, transModel.UserID);
            DatabaseProxy.AddParameter(dbCommand, PARM_MODIFIED_DATE, transModel.ModifiedDate);

            try
            {
                dbConnection.Open();
                insertID = dbCommand.ExecuteScalar();

            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                dbConnection.Close();
            }

            return (int)insertID;
        }
    }
}
