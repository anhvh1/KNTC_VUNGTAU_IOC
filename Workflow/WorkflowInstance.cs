using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workflow.Model;

namespace Workflow
{
    public class WorkflowInstance
    {
        private DataQuery _dataQuery;
        protected DataQuery DataQuery
        {
            get { return _dataQuery ?? (_dataQuery = new DataQuery()); }
        }

        private static readonly WorkflowInstance _instance = new WorkflowInstance();
        public static WorkflowInstance Instance
        {
            get { return _instance; }
        }

        public WorkflowInstance()
        {
            _dataQuery = new DataQuery();
        }

        public int AttachDocument(int documentID, string workflowCode, int userID, Nullable<DateTime> dueDate)
        {
            int insertID = 0;

            DocumentModel docModel = new DocumentModel();
            docModel.DocumentID = documentID;
            docModel.WorkflowID = this.DataQuery.GetWorkflowIDByCode(workflowCode);
            docModel.StateID = this.DataQuery.GetFirstState(workflowCode);
            docModel.DueDate = dueDate;

            insertID = this.DataQuery.InsertDocument(docModel);

            return insertID;
        }
        public bool DeleteDocument(int documentID)
        {
            bool result = false;

            DocumentModel docModel = new DocumentModel();
            docModel.DocumentID = documentID;

            result = this.DataQuery.DeleteDocument(docModel);

            return result;
        }
        public string GetPrevStateOfDocument(int documentID)
        {
            string result = "";
            result = this.DataQuery.GetPrevStateOfDocument(documentID);
            return result;
        }

        public string GetCurrentStateOfDocument(int documentID)
        {
            if (documentID == 0) return string.Empty;

            string result = "";
            result = this.DataQuery.GetCurrentStateOfDocument(documentID);
            return result;
        }

        public List<int> GetDocumentByPrevState(string currentStateName, string nextStateName, DateTime startDate, DateTime endDate)
        {
            return this.DataQuery.GetDocumentByPrevState(currentStateName, nextStateName, startDate, endDate);
        }

        public List<DocumentModel> GetDocumentByState(string stateName, DateTime startDate, DateTime endDate)
        {
            return this.DataQuery.GetDocumentByState(stateName, startDate, endDate);
        }

        public List<string> GetAvailabelCommands(int documentID)
        {
            List<string> commandList = new List<string>();

            DocumentModel docModel = this.DataQuery.GetDocumentByID(documentID);

            var commands = this.DataQuery.GetCommandByState(docModel.StateID);

            foreach (var command in commands)
            {
                commandList.Add(command.CommandCode);
            }

            return commandList;
        }

        public List<string> GetCommandsByStateID(int stateID)
        {
            List<string> commandList = new List<string>();

            var commands = this.DataQuery.GetCommandByState(stateID);

            foreach (var command in commands)
            {
                commandList.Add(command.CommandCode);
            }

            return commandList;
        }

        public bool ExecuteCommand(int documentID, int userID, string commandCode, DateTime dueDate, string comment, int? stateIdGop = null)
        {
            DocumentModel docModel = this.DataQuery.GetDocumentByID(documentID);

            if (docModel == null) return false;

            int currentStateID = (stateIdGop ?? 0) > 0 ? (stateIdGop ?? 0) : docModel.StateID;

            docModel.DocumentID = documentID;
            docModel.StateID = this.DataQuery.GetNextState(currentStateID, commandCode);
            docModel.DueDate = dueDate;

            this.DataQuery.UpdateDocumentState(docModel);

            TransitionHistoryModel transHistoryModel = new TransitionHistoryModel();

            TransitionModel transModel = this.DataQuery.GetTransitionByStateAndCommand(currentStateID, commandCode);

            if (transModel == null) return false;

            transHistoryModel.DueDate = dueDate;
            transHistoryModel.Comment = !string.IsNullOrEmpty(comment) ? comment : "" ;
            transHistoryModel.DocumentID = documentID;
            transHistoryModel.TransitionID = transModel.TransitionID;
            transHistoryModel.UserID = userID;
            transHistoryModel.ModifiedDate = DateTime.Now;

            this.DataQuery.InsertTransitionHistory(transHistoryModel);

            return true;
        }
    }
}
