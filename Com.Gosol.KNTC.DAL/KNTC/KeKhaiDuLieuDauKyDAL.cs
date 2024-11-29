using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class KeKhaiDuLieuDauKyDAL
    {
        List<T> CreateList<T>(params T[] values)
        {
            return new List<T>(values);
        }
        public List<TableData> TCD01(KeKhaiDuLieuDauKyParams p)
        {
            List<TableData> data = new List<TableData>();
            TableData tableData = new TableData();
            tableData.ID = 2;
            var DataArr = new List<RowItem>();     
            string Css = "";
            int tmp = 2 * 100000;
            bool? isEdit = true;

            var cq = new CoQuan().GetCoQuanByID(p.CoQuanID ?? 0);
            KeKhaiDuLieuDauKyModel DuLieuDauKy = new KeKhaiDuLieuDauKyModel();
            DuLieuDauKy = GetDuLieuDauKy(p);
            DuLieuDauKy.CoQuanID = cq.CoQuanID;
            DuLieuDauKy.TenCoQuan = cq.TenCoQuan;
            tableData.CoQuanID = cq.CoQuanID;

            RowItem RowItem1 = new RowItem(1 + tmp, DuLieuDauKy.TenCoQuan, DuLieuDauKy.CoQuanID.ToString(), "", false, "", ref DataArr);
            RowItem RowItem2 = new RowItem(2 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col1.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 5, tmp + 14, tmp + 23), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem3 = new RowItem(3 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col2.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 6, tmp + 15, tmp + 24), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem4 = new RowItem(4 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col3.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 7, tmp + 8, tmp + 16, tmp + 17, tmp + 25, tmp + 26), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem5 = new RowItem(5 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col4.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem6 = new RowItem(6 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col5.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem7 = new RowItem(7 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col6.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem8 = new RowItem(8 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col7.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem9 = new RowItem(9 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col8.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem10 = new RowItem(10 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col9.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem11 = new RowItem(11 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col10.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem12 = new RowItem(12 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col11.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem13 = new RowItem(13 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col12.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem14 = new RowItem(14 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col13.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem15 = new RowItem(15 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col14.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem16 = new RowItem(16 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col15.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem17 = new RowItem(17 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col16.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem18 = new RowItem(18 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col17.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem19 = new RowItem(19 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col18.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem20 = new RowItem(20 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col19.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem21 = new RowItem(21 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col20.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem22 = new RowItem(22 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col21.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem23 = new RowItem(23 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col22.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem24 = new RowItem(24 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col23.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem25 = new RowItem(25 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col24.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem26 = new RowItem(26 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col25.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem27 = new RowItem(27 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col26.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem28 = new RowItem(28 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col27.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem29 = new RowItem(29 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col28.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem30 = new RowItem(30 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col29.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem31 = new RowItem(31 + tmp, DuLieuDauKy.Col30_GhiChu, DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: left;" + Css, TypeEdit.String.GetHashCode(), ref DataArr);
            tableData.DataArr = DataArr.OrderBy(x => x.ID).ToList();

            data.Add(tableData);

            return data;
        }
        public List<TableData> TCD02(KeKhaiDuLieuDauKyParams p)
        {
            List<TableData> data = new List<TableData>();
            TableData tableData = new TableData();
            tableData.ID = 2;
            var DataArr = new List<RowItem>();
            string Css = "";
            int tmp = 2 * 100000;
            bool? isEdit = true;

            var cq = new CoQuan().GetCoQuanByID(p.CoQuanID ?? 0);
            KeKhaiDuLieuDauKyModel DuLieuDauKy = new KeKhaiDuLieuDauKyModel();
            DuLieuDauKy = GetDuLieuDauKy(p);
            DuLieuDauKy.CoQuanID = cq.CoQuanID;
            DuLieuDauKy.TenCoQuan = cq.TenCoQuan;
            tableData.CoQuanID = cq.CoQuanID;

            RowItem RowItem1 = new RowItem(1 + tmp, DuLieuDauKy.TenCoQuan, DuLieuDauKy.CoQuanID.ToString(), "", false, "", ref DataArr);
            RowItem RowItem2 = new RowItem(2 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col1.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 4, tmp + 6, tmp + 8),TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem3 = new RowItem(3 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col2.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 5, tmp + 7, tmp + 9), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem4 = new RowItem(4 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col3.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem5 = new RowItem(5 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col4.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem6 = new RowItem(6 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col5.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem7 = new RowItem(7 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col6.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem8 = new RowItem(8 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col7.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem9 = new RowItem(9 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col8.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem10 = new RowItem(10 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col9.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem11 = new RowItem(11 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col10.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem12 = new RowItem(12 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col11.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem13 = new RowItem(13 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col12.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem14 = new RowItem(14 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col13.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem15 = new RowItem(15 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col14.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem16 = new RowItem(16 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col15.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem17 = new RowItem(17 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col16.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem18 = new RowItem(18 + tmp, DuLieuDauKy.Col17_GhiChu, DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: left;" + Css, TypeEdit.String.GetHashCode(), ref DataArr);
            tableData.DataArr = DataArr.OrderBy(x => x.ID).ToList(); 

            data.Add(tableData);

            return data;
        }
        public List<TableData> XLD01(KeKhaiDuLieuDauKyParams p)
        {
            List<TableData> data = new List<TableData>();
            TableData tableData = new TableData();
            tableData.ID = 2;
            var DataArr = new List<RowItem>();
            string Css = "";
            int tmp = 2 * 100000;
            bool? isEdit = true;

            var cq = new CoQuan().GetCoQuanByID(p.CoQuanID ?? 0);
            KeKhaiDuLieuDauKyModel DuLieuDauKy = new KeKhaiDuLieuDauKyModel();
            DuLieuDauKy = GetDuLieuDauKy(p);
            DuLieuDauKy.CoQuanID = cq.CoQuanID;
            DuLieuDauKy.TenCoQuan = cq.TenCoQuan;
            tableData.CoQuanID = cq.CoQuanID;

            RowItem RowItem1 = new RowItem(1 + tmp, DuLieuDauKy.TenCoQuan, DuLieuDauKy.CoQuanID.ToString(), "", false, "", ref DataArr);
            RowItem RowItem2 = new RowItem(2 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col1.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 3, tmp + 4, tmp + 5, tmp + 6, tmp + 7, tmp + 8), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem3 = new RowItem(3 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col2.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem4 = new RowItem(4 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col3.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem5 = new RowItem(5 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col4.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem6 = new RowItem(6 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col5.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem7 = new RowItem(7 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col6.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem8 = new RowItem(8 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col7.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem9 = new RowItem(9 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col8.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem10 = new RowItem(10 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col9.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 12, tmp + 13, tmp + 14), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem11 = new RowItem(11 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col10.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem12 = new RowItem(12 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col11.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem13 = new RowItem(13 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col12.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem14 = new RowItem(14 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col13.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem15 = new RowItem(15 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col14.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem16 = new RowItem(16 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col15.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem17 = new RowItem(17 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col16.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem18 = new RowItem(18 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col17.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem19 = new RowItem(19 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col18.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 20, tmp + 21, tmp + 22), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem20 = new RowItem(20 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col19.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem21 = new RowItem(21 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col20.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem22 = new RowItem(22 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col21.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem23 = new RowItem(23 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col22.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 24, tmp + 25, tmp + 26), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem24 = new RowItem(24 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col23.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem25 = new RowItem(25 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col24.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem26 = new RowItem(26 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col25.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem27 = new RowItem(27 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col26.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem28 = new RowItem(28 + tmp, DuLieuDauKy.Col27_GhiChu, DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: left;" + Css, TypeEdit.String.GetHashCode(), ref DataArr);
            tableData.DataArr = DataArr.OrderBy(x => x.ID).ToList();

            data.Add(tableData);

            return data;
        }
        public List<TableData> XLD02(KeKhaiDuLieuDauKyParams p)
        {
            List<TableData> data = new List<TableData>();
            TableData tableData = new TableData();
            tableData.ID = 2;
            var DataArr = new List<RowItem>();
            string Css = "";
            int tmp = 2 * 100000;
            bool? isEdit = true;

            var cq = new CoQuan().GetCoQuanByID(p.CoQuanID ?? 0);
            KeKhaiDuLieuDauKyModel DuLieuDauKy = new KeKhaiDuLieuDauKyModel();
            DuLieuDauKy = GetDuLieuDauKy(p);
            DuLieuDauKy.CoQuanID = cq.CoQuanID;
            DuLieuDauKy.TenCoQuan = cq.TenCoQuan;
            tableData.CoQuanID = cq.CoQuanID;

            RowItem RowItem1 = new RowItem(1 + tmp, DuLieuDauKy.TenCoQuan, DuLieuDauKy.CoQuanID.ToString(), "", false, "", ref DataArr);
            RowItem RowItem2 = new RowItem(2 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col1.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 3, tmp + 4, tmp + 5, tmp + 6), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem3 = new RowItem(3 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col2.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem4 = new RowItem(4 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col3.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem5 = new RowItem(5 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col4.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem6 = new RowItem(6 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col5.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem7 = new RowItem(7 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col6.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align:right;" + Css, CreateList(tmp + 8, tmp + 9), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem8 = new RowItem(8 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col7.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem9 = new RowItem(9 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col8.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem10 = new RowItem(10 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col9.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem11 = new RowItem(11 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col10.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 12, tmp + 16, tmp + 17, tmp + 18), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem12 = new RowItem(12 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col11.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem13 = new RowItem(13 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col12.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem14 = new RowItem(14 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col13.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem15 = new RowItem(15 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col14.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem16 = new RowItem(16 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col15.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem17 = new RowItem(17 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col16.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem18 = new RowItem(18 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col17.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem19 = new RowItem(19 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col18.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem20 = new RowItem(20 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col19.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem21 = new RowItem(21 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col20.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem22 = new RowItem(22 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col21.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem23 = new RowItem(23 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col22.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem24 = new RowItem(24 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col23.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 25, tmp + 26), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem25 = new RowItem(25 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col24.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem26 = new RowItem(26 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col25.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem27 = new RowItem(27 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col26.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 28, tmp + 29, tmp + 30), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem28 = new RowItem(28 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col27.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem29 = new RowItem(29 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col28.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem30 = new RowItem(30 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col29.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem31 = new RowItem(31 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col30.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem32 = new RowItem(32 + tmp, DuLieuDauKy.Col31_GhiChu, DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: left;" + Css, TypeEdit.String.GetHashCode(), ref DataArr);
            tableData.DataArr = DataArr.OrderBy(x => x.ID).ToList();

            data.Add(tableData);

            return data;
        }
        public List<TableData> XLD03(KeKhaiDuLieuDauKyParams p)
        {
            List<TableData> data = new List<TableData>();
            TableData tableData = new TableData();
            tableData.ID = 2;
            var DataArr = new List<RowItem>();
            string Css = "";
            int tmp = 2 * 100000;
            bool? isEdit = true;

            var cq = new CoQuan().GetCoQuanByID(p.CoQuanID ?? 0);
            KeKhaiDuLieuDauKyModel DuLieuDauKy = new KeKhaiDuLieuDauKyModel();
            DuLieuDauKy = GetDuLieuDauKy(p);
            DuLieuDauKy.CoQuanID = cq.CoQuanID;
            DuLieuDauKy.TenCoQuan = cq.TenCoQuan;
            tableData.CoQuanID = cq.CoQuanID;

            RowItem RowItem1 = new RowItem(1 + tmp, DuLieuDauKy.TenCoQuan, DuLieuDauKy.CoQuanID.ToString(), "", false, "", ref DataArr);
            RowItem RowItem2 = new RowItem(2 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col1.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 3, tmp + 4, tmp + 5, tmp + 6, tmp + 7, tmp + 8), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem3 = new RowItem(3 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col2.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem4 = new RowItem(4 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col3.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem5 = new RowItem(5 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col4.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem6 = new RowItem(6 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col5.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem7 = new RowItem(7 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col6.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css,TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem8 = new RowItem(8 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col7.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem9 = new RowItem(9 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col8.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem10 = new RowItem(10 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col9.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem11 = new RowItem(11 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col10.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem12 = new RowItem(12 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col11.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem13 = new RowItem(13 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col12.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 14, tmp + 19, tmp + 20, tmp + 21, tmp + 22), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem14 = new RowItem(14 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col13.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem15 = new RowItem(15 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col14.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem16 = new RowItem(16 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col15.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem17 = new RowItem(17 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col16.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem18 = new RowItem(18 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col17.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem19 = new RowItem(19 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col18.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem20 = new RowItem(20 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col19.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem21 = new RowItem(21 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col20.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem22 = new RowItem(22 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col21.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem23 = new RowItem(23 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col22.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem24 = new RowItem(24 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col23.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem25 = new RowItem(25 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col24.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem26 = new RowItem(26 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col25.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem27 = new RowItem(27 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col26.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 28, tmp + 29), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem28 = new RowItem(28 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col27.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem29 = new RowItem(29 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col28.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem30 = new RowItem(30 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col29.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 31, tmp + 32, tmp + 33), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem31 = new RowItem(31 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col30.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem32 = new RowItem(32 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col31.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem33 = new RowItem(33 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col32.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem34 = new RowItem(34 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col33.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem35 = new RowItem(35 + tmp, DuLieuDauKy.Col34_GhiChu, DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: left;" + Css, TypeEdit.String.GetHashCode(), ref DataArr);
            tableData.DataArr = DataArr.OrderBy(x => x.ID).ToList();

            data.Add(tableData);

            return data;
        }
        public List<TableData> XLD04(KeKhaiDuLieuDauKyParams p)
        {
            List<TableData> data = new List<TableData>();
            TableData tableData = new TableData();
            tableData.ID = 2;
            var DataArr = new List<RowItem>();
            string Css = "";
            int tmp = 2 * 100000;
            bool? isEdit = true;

            var cq = new CoQuan().GetCoQuanByID(p.CoQuanID ?? 0);
            KeKhaiDuLieuDauKyModel DuLieuDauKy = new KeKhaiDuLieuDauKyModel();
            DuLieuDauKy = GetDuLieuDauKy(p);
            DuLieuDauKy.CoQuanID = cq.CoQuanID;
            DuLieuDauKy.TenCoQuan = cq.TenCoQuan;
            tableData.CoQuanID = cq.CoQuanID;

            RowItem RowItem1 = new RowItem(1 + tmp, DuLieuDauKy.TenCoQuan, DuLieuDauKy.CoQuanID.ToString(), "", false, "", ref DataArr);
            RowItem RowItem2 = new RowItem(2 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col1.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 3, tmp + 4, tmp + 5, tmp + 6, tmp + 7, tmp + 8), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem3 = new RowItem(3 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col2.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem4 = new RowItem(4 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col3.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem5 = new RowItem(5 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col4.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem6 = new RowItem(6 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col5.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem7 = new RowItem(7 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col6.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem8 = new RowItem(8 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col7.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem9 = new RowItem(9 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col8.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 10, tmp + 11), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem10 = new RowItem(10 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col9.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem11 = new RowItem(11 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col10.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem12 = new RowItem(12 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col11.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem13 = new RowItem(13 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col12.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 14, tmp + 15, tmp + 16, tmp + 17), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem14 = new RowItem(14 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col13.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem15 = new RowItem(15 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col14.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem16 = new RowItem(16 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col15.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem17 = new RowItem(17 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col16.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem18 = new RowItem(18 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col17.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem19 = new RowItem(19 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col18.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem20 = new RowItem(20 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col19.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem21 = new RowItem(21 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col20.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem22 = new RowItem(22 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col21.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 23, tmp + 24), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem23 = new RowItem(23 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col22.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem24 = new RowItem(24 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col23.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem25 = new RowItem(25 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col24.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem26 = new RowItem(26 + tmp, DuLieuDauKy.Col25_GhiChu, DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: left;" + Css, TypeEdit.String.GetHashCode(), ref DataArr);
            tableData.DataArr = DataArr.OrderBy(x => x.ID).ToList();

            data.Add(tableData);

            return data;
        }
        public List<TableData> KQGQ01(KeKhaiDuLieuDauKyParams p)
        {
            List<TableData> data = new List<TableData>();
            TableData tableData = new TableData();
            tableData.ID = 2;
            var DataArr = new List<RowItem>();
            string Css = "";
            int tmp = 2 * 100000;
            bool? isEdit = true;

            var cq = new CoQuan().GetCoQuanByID(p.CoQuanID ?? 0);
            KeKhaiDuLieuDauKyModel DuLieuDauKy = new KeKhaiDuLieuDauKyModel();
            DuLieuDauKy = GetDuLieuDauKy(p);
            DuLieuDauKy.CoQuanID = cq.CoQuanID;
            DuLieuDauKy.TenCoQuan = cq.TenCoQuan;
            tableData.CoQuanID = cq.CoQuanID;

            RowItem RowItem1 = new RowItem(1 + tmp, DuLieuDauKy.TenCoQuan, DuLieuDauKy.CoQuanID.ToString(), "", false, "", ref DataArr);
            RowItem RowItem2 = new RowItem(2 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col1.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 3, tmp + 4), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem3 = new RowItem(3 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col2.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem4 = new RowItem(4 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col3.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem5 = new RowItem(5 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col4.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem6 = new RowItem(6 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col5.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align:right;" + Css, CreateList(tmp + 21, tmp + 22, tmp + 23, tmp + 24, tmp + 25), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem7 = new RowItem(7 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col6.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem8 = new RowItem(8 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col7.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem9 = new RowItem(9 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col8.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem10 = new RowItem(10 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col9.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem11 = new RowItem(11 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col10.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem12 = new RowItem(12 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col11.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem13 = new RowItem(13 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col12.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem14 = new RowItem(14 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col13.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem15 = new RowItem(15 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col14.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem16 = new RowItem(16 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col15.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem17 = new RowItem(17 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col16.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem18 = new RowItem(18 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col17.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem19 = new RowItem(19 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col18.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem20 = new RowItem(20 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col19.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem21 = new RowItem(21 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col20.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem22 = new RowItem(22 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col21.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem23 = new RowItem(23 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col22.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem24 = new RowItem(24 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col23.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem25 = new RowItem(25 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col24.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem26 = new RowItem(26 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col25.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem27 = new RowItem(27 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col26.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem28 = new RowItem(28 + tmp, DuLieuDauKy.Col27_GhiChu, DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: left;" + Css, TypeEdit.String.GetHashCode(), ref DataArr);
           
            tableData.DataArr = DataArr.OrderBy(x => x.ID).ToList();

            data.Add(tableData);

            return data;
        }
        public List<TableData> KQGQ02(KeKhaiDuLieuDauKyParams p)
        {
            List<TableData> data = new List<TableData>();
            TableData tableData = new TableData();
            tableData.ID = 2;
            var DataArr = new List<RowItem>();
            string Css = "";
            int tmp = 2 * 100000;
            bool? isEdit = true;

            var cq = new CoQuan().GetCoQuanByID(p.CoQuanID ?? 0);
            KeKhaiDuLieuDauKyModel DuLieuDauKy = new KeKhaiDuLieuDauKyModel();
            DuLieuDauKy = GetDuLieuDauKy(p);
            DuLieuDauKy.CoQuanID = cq.CoQuanID;
            DuLieuDauKy.TenCoQuan = cq.TenCoQuan;
            tableData.CoQuanID = cq.CoQuanID;

            RowItem RowItem1 = new RowItem(1 + tmp, DuLieuDauKy.TenCoQuan, DuLieuDauKy.CoQuanID.ToString(), "", false, "", ref DataArr);
            RowItem RowItem2 = new RowItem(2 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col1.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem3 = new RowItem(3 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col2.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css,TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem4 = new RowItem(4 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col3.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem5 = new RowItem(5 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col4.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem6 = new RowItem(6 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col5.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem7 = new RowItem(7 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col6.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem8 = new RowItem(8 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col7.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem9 = new RowItem(9 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col8.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem10 = new RowItem(10 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col9.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem11 = new RowItem(11 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col10.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem12 = new RowItem(12 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col11.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem13 = new RowItem(13 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col12.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem14 = new RowItem(14 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col13.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem15 = new RowItem(15 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col14.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem16 = new RowItem(16 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col15.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem17 = new RowItem(17 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col16.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem18 = new RowItem(18 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col17.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem19 = new RowItem(19 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col18.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem20 = new RowItem(20 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col19.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem21 = new RowItem(21 + tmp,DuLieuDauKy.Col20_GhiChu, DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: left;" + Css, TypeEdit.String.GetHashCode(), ref DataArr);
           
            tableData.DataArr = DataArr.OrderBy(x => x.ID).ToList();

            data.Add(tableData);

            return data;
        }
        public List<TableData> KQGQ03(KeKhaiDuLieuDauKyParams p)
        {
            List<TableData> data = new List<TableData>();
            TableData tableData = new TableData();
            tableData.ID = 2;
            var DataArr = new List<RowItem>();
            string Css = "";
            int tmp = 2 * 100000;
            bool? isEdit = true;

            var cq = new CoQuan().GetCoQuanByID(p.CoQuanID ?? 0);
            KeKhaiDuLieuDauKyModel DuLieuDauKy = new KeKhaiDuLieuDauKyModel();
            DuLieuDauKy = GetDuLieuDauKy(p);
            DuLieuDauKy.CoQuanID = cq.CoQuanID;
            DuLieuDauKy.TenCoQuan = cq.TenCoQuan;
            tableData.CoQuanID = cq.CoQuanID;

            RowItem RowItem1 = new RowItem(1 + tmp, DuLieuDauKy.TenCoQuan, DuLieuDauKy.CoQuanID.ToString(), "", false, "", ref DataArr);
            RowItem RowItem2 = new RowItem(2 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col1.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 3, tmp + 4), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem3 = new RowItem(3 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col2.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem4 = new RowItem(4 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col3.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem5 = new RowItem(5 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col4.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem6 = new RowItem(6 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col5.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem7 = new RowItem(7 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col6.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align:right;" + Css, CreateList(tmp + 8, tmp + 9, tmp + 10, tmp + 11), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem8 = new RowItem(8 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col7.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem9 = new RowItem(9 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col8.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", false, "text-align: right;" + Css, CreateList(tmp + 26, tmp + 28, tmp + 30), TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem10 = new RowItem(10 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col9.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem11 = new RowItem(11 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col10.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem12 = new RowItem(12 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col11.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem13 = new RowItem(13 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col12.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem14 = new RowItem(14 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col13.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem15 = new RowItem(15 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col14.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem16 = new RowItem(16 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col15.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem17 = new RowItem(17 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col16.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem18 = new RowItem(18 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col17.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem19 = new RowItem(19 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col18.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem20 = new RowItem(20 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col19.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem21 = new RowItem(21 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col20.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem22 = new RowItem(22 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col21.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem23 = new RowItem(23 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col22.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem24 = new RowItem(24 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col23.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem25 = new RowItem(25 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col24.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem26 = new RowItem(26 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col25.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem27 = new RowItem(27 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col26.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem28 = new RowItem(28 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col27.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem29 = new RowItem(29 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col28.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem30 = new RowItem(30 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col29.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem31 = new RowItem(31 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col30.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem32 = new RowItem(32 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col31.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem33 = new RowItem(33 + tmp, DuLieuDauKy.Col32_GhiChu, DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: left;" + Css, TypeEdit.String.GetHashCode(), ref DataArr);
           
            tableData.DataArr = DataArr.OrderBy(x => x.ID).ToList();

            data.Add(tableData);

            return data;
        }
        public List<TableData> KQGQ04(KeKhaiDuLieuDauKyParams p)
        {
            List<TableData> data = new List<TableData>();
            TableData tableData = new TableData();
            tableData.ID = 2;
            var DataArr = new List<RowItem>();
            string Css = "";
            int tmp = 2 * 100000;
            bool? isEdit = true;

            var cq = new CoQuan().GetCoQuanByID(p.CoQuanID ?? 0);
            KeKhaiDuLieuDauKyModel DuLieuDauKy = new KeKhaiDuLieuDauKyModel();
            DuLieuDauKy = GetDuLieuDauKy(p);
            DuLieuDauKy.CoQuanID = cq.CoQuanID;
            DuLieuDauKy.TenCoQuan = cq.TenCoQuan;
            tableData.CoQuanID = cq.CoQuanID;

            RowItem RowItem1 = new RowItem(1 + tmp, DuLieuDauKy.TenCoQuan, DuLieuDauKy.CoQuanID.ToString(), "", false, "", ref DataArr);
            RowItem RowItem2 = new RowItem(2 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col1.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem3 = new RowItem(3 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col2.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem4 = new RowItem(4 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col3.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem5 = new RowItem(5 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col4.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem6 = new RowItem(6 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col5.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem7 = new RowItem(7 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col6.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem8 = new RowItem(8 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col7.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align:right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem9 = new RowItem(9 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col8.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem10 = new RowItem(10 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col9.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem11 = new RowItem(11 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col10.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem12 = new RowItem(12 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col11.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem13 = new RowItem(13 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col12.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem14 = new RowItem(14 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col13.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem15 = new RowItem(15 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col14.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem16 = new RowItem(16 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col15.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem17 = new RowItem(17 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col16.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem18 = new RowItem(18 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col17.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem19 = new RowItem(19 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col18.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem20 = new RowItem(20 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col19.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem21 = new RowItem(21 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col20.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem22 = new RowItem(22 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col21.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem23 = new RowItem(23 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col22.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem24 = new RowItem(24 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col23.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(),ref DataArr);
            RowItem RowItem25 = new RowItem(25 + tmp, Utils.AddCommasDouble(DuLieuDauKy.Col24.ToString()), DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: right;" + Css, TypeEdit.Number.GetHashCode(), ref DataArr);
            RowItem RowItem26 = new RowItem(26 + tmp, DuLieuDauKy.Col25_GhiChu, DuLieuDauKy.CoQuanID.ToString(), "", isEdit, "text-align: left;" + Css, TypeEdit.String.GetHashCode(), ref DataArr);
            
            tableData.DataArr = DataArr;

            data.Add(tableData);

            return data;
        }
        public KeKhaiDuLieuDauKyModel GetDuLieuDauKy(KeKhaiDuLieuDauKyParams p)
        {

            KeKhaiDuLieuDauKyModel Info = new KeKhaiDuLieuDauKyModel();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("CoQuanID",SqlDbType.Int),
                new SqlParameter("LoaiBaoCao",SqlDbType.Int),
                new SqlParameter("NgaySuDung",SqlDbType.DateTime),
            };
            parameters[0].Value = p.CoQuanID ?? Convert.DBNull;
            parameters[1].Value = p.LoaiBaoCao ?? Convert.DBNull;
            parameters[2].Value = p.NgaySuDung ?? Convert.DBNull; 

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_KeKhaiDuLieuDauKy_GetDuLieuDauKy", parameters))
                {

                    if (dr.Read())
                    {
                        Info.ID = Utils.ConvertToInt32(dr["ID"], 0);
                        Info.LoaiBaoCao = Utils.ConvertToInt32(dr["LoaiBaoCao"], 0);
                        Info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        Info.NgaySuDung = Utils.ConvertToNullableDateTime(dr["NgaySuDung"], null);
                        Info.Col1 = Utils.ConvertToDecimal(dr["Col1"], 0);
                        Info.Col2 = Utils.ConvertToDecimal(dr["Col2"], 0);
                        Info.Col3 = Utils.ConvertToDecimal(dr["Col3"], 0);
                        Info.Col4 = Utils.ConvertToDecimal(dr["Col4"], 0);
                        Info.Col5 = Utils.ConvertToDecimal(dr["Col5"], 0);
                        Info.Col6 = Utils.ConvertToDecimal(dr["Col6"], 0);
                        Info.Col7 = Utils.ConvertToDecimal(dr["Col7"], 0);
                        Info.Col8 = Utils.ConvertToDecimal(dr["Col8"], 0);
                        Info.Col9 = Utils.ConvertToDecimal(dr["Col9"], 0);
                        Info.Col10 = Utils.ConvertToDecimal(dr["Col10"], 0);
                        Info.Col11 = Utils.ConvertToDecimal(dr["Col11"], 0);
                        Info.Col12 = Utils.ConvertToDecimal(dr["Col12"], 0);
                        Info.Col13 = Utils.ConvertToDecimal(dr["Col13"], 0);
                        Info.Col14 = Utils.ConvertToDecimal(dr["Col14"], 0);
                        Info.Col15 = Utils.ConvertToDecimal(dr["Col15"], 0);
                        Info.Col16 = Utils.ConvertToDecimal(dr["Col16"], 0);
                        Info.Col17 = Utils.ConvertToDecimal(dr["Col17"], 0);
                        Info.Col18 = Utils.ConvertToDecimal(dr["Col18"], 0);
                        Info.Col19 = Utils.ConvertToDecimal(dr["Col19"], 0);
                        Info.Col20 = Utils.ConvertToDecimal(dr["Col20"], 0);
                        Info.Col21 = Utils.ConvertToDecimal(dr["Col21"], 0);
                        Info.Col22 = Utils.ConvertToDecimal(dr["Col22"], 0);
                        Info.Col23 = Utils.ConvertToDecimal(dr["Col23"], 0);
                        Info.Col24 = Utils.ConvertToDecimal(dr["Col24"], 0);
                        Info.Col25 = Utils.ConvertToDecimal(dr["Col25"], 0);
                        Info.Col26 = Utils.ConvertToDecimal(dr["Col26"], 0);
                        Info.Col27 = Utils.ConvertToDecimal(dr["Col27"], 0);
                        Info.Col28 = Utils.ConvertToDecimal(dr["Col28"], 0);
                        Info.Col29 = Utils.ConvertToDecimal(dr["Col29"], 0);
                        Info.Col30 = Utils.ConvertToDecimal(dr["Col30"], 0);
                        Info.Col31 = Utils.ConvertToDecimal(dr["Col31"], 0);
                        Info.Col32 = Utils.ConvertToDecimal(dr["Col32"], 0);
                        Info.Col33 = Utils.ConvertToDecimal(dr["Col33"], 0);
                        Info.Col34 = Utils.ConvertToDecimal(dr["Col34"], 0);
                        Info.Col35 = Utils.ConvertToDecimal(dr["Col35"], 0);
                        Info.Col17_GhiChu = Utils.ConvertToString(dr["Col17_GhiChu"], string.Empty);
                        Info.Col18_GhiChu = Utils.ConvertToString(dr["Col18_GhiChu"], string.Empty);
                        Info.Col19_GhiChu = Utils.ConvertToString(dr["Col19_GhiChu"], string.Empty);
                        Info.Col20_GhiChu = Utils.ConvertToString(dr["Col20_GhiChu"], string.Empty);
                        Info.Col21_GhiChu = Utils.ConvertToString(dr["Col21_GhiChu"], string.Empty);
                        Info.Col22_GhiChu = Utils.ConvertToString(dr["Col22_GhiChu"], string.Empty);
                        Info.Col23_GhiChu = Utils.ConvertToString(dr["Col23_GhiChu"], string.Empty);
                        Info.Col24_GhiChu = Utils.ConvertToString(dr["Col24_GhiChu"], string.Empty);
                        Info.Col25_GhiChu = Utils.ConvertToString(dr["Col25_GhiChu"], string.Empty);
                        Info.Col26_GhiChu = Utils.ConvertToString(dr["Col26_GhiChu"], string.Empty);
                        Info.Col27_GhiChu = Utils.ConvertToString(dr["Col27_GhiChu"], string.Empty);
                        Info.Col28_GhiChu = Utils.ConvertToString(dr["Col28_GhiChu"], string.Empty);
                        Info.Col29_GhiChu = Utils.ConvertToString(dr["Col29_GhiChu"], string.Empty);
                        Info.Col30_GhiChu = Utils.ConvertToString(dr["Col30_GhiChu"], string.Empty);
                        Info.Col31_GhiChu = Utils.ConvertToString(dr["Col31_GhiChu"], string.Empty);
                        Info.Col32_GhiChu = Utils.ConvertToString(dr["Col32_GhiChu"], string.Empty);
                        Info.Col33_GhiChu = Utils.ConvertToString(dr["Col33_GhiChu"], string.Empty);
                        Info.Col34_GhiChu = Utils.ConvertToString(dr["Col34_GhiChu"], string.Empty);            
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return Info;
        }

        public int Insert(KeKhaiDuLieuDauKyModel Info)
        {
            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("LoaiBaoCao", SqlDbType.Int),
                new SqlParameter("CoQuanID", SqlDbType.Int),
                new SqlParameter("NgaySuDung", SqlDbType.DateTime),
                new SqlParameter("Col1", SqlDbType.Decimal),
                new SqlParameter("Col2", SqlDbType.Decimal),
                new SqlParameter("Col3", SqlDbType.Decimal),
                new SqlParameter("Col4", SqlDbType.Decimal),
                new SqlParameter("Col5", SqlDbType.Decimal),
                new SqlParameter("Col6", SqlDbType.Decimal),
                new SqlParameter("Col7", SqlDbType.Decimal),
                new SqlParameter("Col8", SqlDbType.Decimal),
                new SqlParameter("Col9", SqlDbType.Decimal),
                new SqlParameter("Col10", SqlDbType.Decimal),
                new SqlParameter("Col11", SqlDbType.Decimal),
                new SqlParameter("Col12", SqlDbType.Decimal),
                new SqlParameter("Col13", SqlDbType.Decimal),
                new SqlParameter("Col14", SqlDbType.Decimal),
                new SqlParameter("Col15", SqlDbType.Decimal),
                new SqlParameter("Col16", SqlDbType.Decimal),
                new SqlParameter("Col17", SqlDbType.Decimal),
                new SqlParameter("Col18", SqlDbType.Decimal),
                new SqlParameter("Col19", SqlDbType.Decimal),
                new SqlParameter("Col20", SqlDbType.Decimal),
                new SqlParameter("Col21", SqlDbType.Decimal),
                new SqlParameter("Col22", SqlDbType.Decimal),
                new SqlParameter("Col23", SqlDbType.Decimal),
                new SqlParameter("Col24", SqlDbType.Decimal),
                new SqlParameter("Col25", SqlDbType.Decimal),
                new SqlParameter("Col26", SqlDbType.Decimal),
                new SqlParameter("Col27", SqlDbType.Decimal),
                new SqlParameter("Col28", SqlDbType.Decimal),
                new SqlParameter("Col29", SqlDbType.Decimal),
                new SqlParameter("Col30", SqlDbType.Decimal),
                new SqlParameter("Col31", SqlDbType.Decimal),
                new SqlParameter("Col32", SqlDbType.Decimal),
                new SqlParameter("Col33", SqlDbType.Decimal),
                new SqlParameter("Col34", SqlDbType.Decimal),
                new SqlParameter("Col35", SqlDbType.Decimal),
                new SqlParameter("Col17_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col18_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col19_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col20_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col21_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col22_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col23_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col24_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col25_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col26_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col27_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col28_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col29_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col30_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col31_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col32_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col33_GhiChu", SqlDbType.NVarChar),
                new SqlParameter("Col34_GhiChu", SqlDbType.NVarChar),
            };
            parms[0].Value = Info.LoaiBaoCao ?? Convert.DBNull;
            parms[1].Value = Info.CoQuanID ?? Convert.DBNull;
            parms[2].Value = Info.NgaySuDung ?? DateTime.Now;
            parms[3].Value = Info.Col1 ?? Convert.DBNull;
            parms[4].Value = Info.Col2 ?? Convert.DBNull;
            parms[5].Value = Info.Col3 ?? Convert.DBNull;
            parms[6].Value = Info.Col4 ?? Convert.DBNull;
            parms[7].Value = Info.Col5 ?? Convert.DBNull;
            parms[8].Value = Info.Col6 ?? Convert.DBNull;
            parms[9].Value = Info.Col7 ?? Convert.DBNull;
            parms[10].Value = Info.Col8 ?? Convert.DBNull;
            parms[11].Value = Info.Col9 ?? Convert.DBNull;
            parms[12].Value = Info.Col10 ?? Convert.DBNull;
            parms[13].Value = Info.Col11 ?? Convert.DBNull;
            parms[14].Value = Info.Col12 ?? Convert.DBNull;
            parms[15].Value = Info.Col13 ?? Convert.DBNull;
            parms[16].Value = Info.Col14 ?? Convert.DBNull;
            parms[17].Value = Info.Col15 ?? Convert.DBNull;
            parms[18].Value = Info.Col16 ?? Convert.DBNull;
            parms[19].Value = Info.Col17 ?? Convert.DBNull;
            parms[20].Value = Info.Col18 ?? Convert.DBNull;
            parms[21].Value = Info.Col19 ?? Convert.DBNull;
            parms[22].Value = Info.Col20 ?? Convert.DBNull;
            parms[23].Value = Info.Col21 ?? Convert.DBNull;
            parms[24].Value = Info.Col22 ?? Convert.DBNull;
            parms[25].Value = Info.Col23 ?? Convert.DBNull;
            parms[26].Value = Info.Col24 ?? Convert.DBNull;
            parms[27].Value = Info.Col25 ?? Convert.DBNull;
            parms[28].Value = Info.Col26 ?? Convert.DBNull;
            parms[29].Value = Info.Col27 ?? Convert.DBNull;
            parms[30].Value = Info.Col28 ?? Convert.DBNull;
            parms[31].Value = Info.Col29 ?? Convert.DBNull;
            parms[32].Value = Info.Col30 ?? Convert.DBNull;
            parms[33].Value = Info.Col31 ?? Convert.DBNull;
            parms[34].Value = Info.Col32 ?? Convert.DBNull;
            parms[35].Value = Info.Col33 ?? Convert.DBNull;
            parms[36].Value = Info.Col34 ?? Convert.DBNull;
            parms[37].Value = Info.Col35 ?? Convert.DBNull;
            parms[38].Value = Info.Col17_GhiChu ?? Convert.DBNull;
            parms[39].Value = Info.Col18_GhiChu ?? Convert.DBNull;
            parms[40].Value = Info.Col19_GhiChu ?? Convert.DBNull;
            parms[41].Value = Info.Col20_GhiChu ?? Convert.DBNull;
            parms[42].Value = Info.Col21_GhiChu ?? Convert.DBNull;
            parms[43].Value = Info.Col22_GhiChu ?? Convert.DBNull;
            parms[44].Value = Info.Col23_GhiChu ?? Convert.DBNull;
            parms[45].Value = Info.Col24_GhiChu ?? Convert.DBNull;
            parms[46].Value = Info.Col25_GhiChu ?? Convert.DBNull;
            parms[47].Value = Info.Col26_GhiChu ?? Convert.DBNull;
            parms[48].Value = Info.Col27_GhiChu ?? Convert.DBNull;
            parms[49].Value = Info.Col28_GhiChu ?? Convert.DBNull;
            parms[50].Value = Info.Col29_GhiChu ?? Convert.DBNull;
            parms[51].Value = Info.Col30_GhiChu ?? Convert.DBNull;
            parms[52].Value = Info.Col31_GhiChu ?? Convert.DBNull;
            parms[53].Value = Info.Col32_GhiChu ?? Convert.DBNull;
            parms[54].Value = Info.Col33_GhiChu ?? Convert.DBNull;
            parms[55].Value = Info.Col34_GhiChu ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "v2_KeKhaiDuLieuDauKy_Insert", parms);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return Convert.ToInt32(val);
        }
    }
}
