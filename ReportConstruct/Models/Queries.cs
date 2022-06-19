using ClosedXML.Excel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ReportConstruct.Models
{
    public partial class Queries
    {
      public Queries() { }


        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Запрос")]
        public string Query { get; set; }
        public string Id { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        public virtual List<Params> Params { get; set; }
        public virtual List<ReportFields> ReportFields { get; set; }
        [NotMapped]
        public string ConnectionString { get; set; }
        public byte[] GetExcel()
        {

            return ToExcel(GetRespAsyn(SendRequest()));
        }

        private string SendRequest()
        {
            string query = Query;
            for (int i = 0; i < Params.Count; i++)
            {
                query = query.Replace(Params[i].ParamName, Params[i].ParamValue);
            }

            string constr = ConnectionString;
            OracleConnection con = new OracleConnection();
            con.ConnectionString = constr;
            OracleCommand cmd = new OracleCommand("FLEX_INSERT_CMD");
            cmd.Connection = con;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("ins_cmd", val: query);

            OracleParameter outParam = new OracleParameter();
            outParam = cmd.Parameters.Add("CURS", OracleDbType.RefCursor, ParameterDirection.Output);


            con.Open();
            OracleDataReader rd = cmd.ExecuteReader();
            
            

            string resId="";
            while (rd.Read())
            {
                resId = rd.GetValue(0).ToString();
            }
            rd.Close();
            con.Close();
            return resId;           
        }

        private string GetResponse(string ResId,out string Res)
        {
            string constr = ConnectionString;
            OracleConnection con = new OracleConnection();
            con.ConnectionString = constr;
            OracleCommand cmd = new OracleCommand("FLEX_GET_RESPONSE");
            cmd.Connection = con;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("ins_id", val: ResId);
            OracleParameter outParam = new OracleParameter();
            outParam = cmd.Parameters.Add("CURS", OracleDbType.RefCursor, ParameterDirection.Output);


            con.Open();
            OracleDataReader rd = cmd.ExecuteReader();
            Res = "";
            string resCode="";
            while (rd.Read())
            {
                Res = rd.GetValue(0).ToString();
                resCode = rd.GetValue(1).ToString();
            }           
            con.Close();
            return resCode;

        }

        private string GetRespAsyn(string ResId)
        {
            string resCode="0";
            string res="";
            while (resCode == "0")
            {                
                resCode = GetResponse(ResId,out res);
                Task.Delay(100).Wait();
            }
            return res;
        }

        private byte[] ToExcel(string Res)
        {
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Report");
                try
                {
                    
                    //for (int i = 0; i < ReportFields.Count; i++)
                    //{
                    //    worksheet.Cell(1, i).Value = ReportFields[i].DisplayName;
                    //}

                    using (JsonDocument document = JsonDocument.Parse(Res))
                    {

                        int i = 2;
                        int j = 1;
                        int index;
                        //document.RootElement.EnumerateArray().MoveNext();
                        JsonElement elem = document.RootElement[0];

                        foreach (JsonProperty prop in elem.EnumerateObject())
                        {
                            index = ReportFields.FindIndex(f => f.QueryField == prop.Name);
                            worksheet.Cell(1, j).Value = (index == -1) ? prop.Name : ReportFields[index].DisplayName;
                            j++;
                        }
                        foreach (JsonElement element in document.RootElement.EnumerateArray())
                        {
                            j = 1;
                            foreach (JsonProperty el in element.EnumerateObject())
                            {
                                worksheet.Cell(i, j).Value = el.Value;
                                j++;
                            }
                            i++;
                        }

                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        byte[] content = stream.ToArray();
                        return content;
                    }
                }catch(Exception e)
                {
                    worksheet.Cell(1, 1).Value = e.Message;
                    worksheet.Cell(2, 1).Value = Res;
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        byte[] content = stream.ToArray();
                        return content;
                    }
                }
            }

        }
    }
}
