using System;
using System.Collections.Generic;
using Portal.Models.DataObjects;
using System.Data;
using System.Data.SqlClient;
using Portal.Models;

namespace Portal.DBLayer
{
    public class PortalDB : IPortalDB
    {
        private readonly string connStr;
        private readonly int year;

        public PortalDB(DataConfig conf)
        {
            connStr = conf.ConnectionString;
            year = conf.Year;
        }

        public IEnumerable<EnrolleeDO> Enrollees(string date, string time)
        {
            var data = FetchEnrollees(date,time);
            var count = data.Rows.Count;
            var result = new EnrolleeDO[count];

            for(int i = 0; i < count; i++)
            {
                result[i] = EnrolleeDO.FromDataRow(data.Rows[i]);
            }

            return result;
        }

        public IEnumerable<AchievementDO> Achievements
        {
            get
            {
                var data = FetchAchievements();
                var count = data.Rows.Count;
                var result = new AchievementDO[count];

                for (int i = 0; i < count; i++)
                {
                    result[i] = AchievementDO.FromDataRow(data.Rows[i]);
                }

                return result;
            }
        }

        public IEnumerable<ExamEntryDO> Exams
        {
            get
            {
                var data = FetchExams();
                var count = data.Rows.Count;
                var result = new ExamEntryDO[count];

                for(int i = 0; i < count; i++)
                {
                    result[i] = ExamEntryDO.FromDataRow(data.Rows[i]);
                }
                
                return result;
            }
        }

        public IEnumerable<PlanEntryDO> Plans
        {
            get
            {
                var data = FetchPlans();
                var count = data.Rows.Count;
                var result = new PlanEntryDO[count];

                for(int i = 0; i < count; i++)
                {
                    result[i] = PlanEntryDO.FromDataRow(data.Rows[i]);
                }

                return result;
            }
        }

        public DynamicConfigDO DynamicConfig
        {
            get
            {
                //will throw exception if data null
                var data = FetchDynamicConfig();

                return new DynamicConfigDO
                {
                    UpdateIntervalSec = int.Parse((string)data.Rows[0]["portal_update_interval"]),
                    IsOnlineRegisterAvailable = byte.Parse((string)data.Rows[0]["portal_registration_avalible"]) == 1 ? true : false
                };
            }
        }

        public long CreatePortalEnrollee(string name, string xmlString)
        {
            Int64 res_value = 0;
            string SQLtext = "use insignia\n";
            SQLtext += "exec [dbo].[_A_SCD_AddUpdateDeleteAbiturientPortal]";
            SQLtext += "@type";
            SQLtext += ",@nrec_portal output";
            SQLtext += ",@fio_abit";
            SQLtext += ",@nrec_abit";
            SQLtext += ",@statements";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand SQLListAll = new SqlCommand(SQLtext, conn);
                SQLListAll.CommandTimeout = 3600;
                SQLListAll.Parameters.AddWithValue("@type", 0);
                SQLListAll.Parameters.AddWithValue("@nrec_portal", res_value).Direction = ParameterDirection.InputOutput;
                SQLListAll.Parameters.AddWithValue("@fio_abit", name);
                SQLListAll.Parameters.AddWithValue("@nrec_abit", 0);
                SQLListAll.Parameters.AddWithValue("@statements", xmlString);

                try
                {
                    conn.Open();
                    SQLListAll.ExecuteReader();
                    res_value = Convert.ToInt64(SQLListAll.Parameters["@nrec_portal"].Value);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    //ServiceForms.Errors.ShowError(ex);
                }
            }
            return res_value;
        }

        #region Fetch
        private DataTable FetchAchievements()
        {
            var result = new DataTable();

            string sqlText = "exec _A_SCD 'portal','0',@year,0";

            using (var conn = new SqlConnection(connStr))
            {
                var command = new SqlCommand(sqlText, conn);
                command.Parameters.AddWithValue("@year", year);
                try
                {
                    conn.Open();
                    result.Load(command.ExecuteReader());
                    conn.Close();
                } catch { }
            }

            return result;
        }

        private DataTable FetchPlans()
        {
            var result = new DataTable();

            string sqlText = "select * from _getUPlanList(8, @year, -1, '', -1)";

            using (var conn = new SqlConnection(connStr))
            {
                var command = new SqlCommand(sqlText, conn);
                command.Parameters.AddWithValue("@year", year);
                try
                {
                    conn.Open();
                    result.Load(command.ExecuteReader());
                    conn.Close();
                } catch { }
            }

            return result;
        }

        private DataTable FetchExams()
        {
            var result = new DataTable();
            var sqlText = "exec _A_SCD 'portal','1',@year,0";

            using (var conn = new SqlConnection(connStr))
            {
                var command = new SqlCommand(sqlText, conn);
                command.Parameters.AddWithValue("@year", year);
                try
                {
                    conn.Open();
                    result.Load(command.ExecuteReader());
                    conn.Close();
                } catch { }
            }

            return result;
        }

        private DataTable FetchEnrollees(string dateIn = "", string timeIn = "")
        {
            var result = new DataTable();
            var sqlText = "exec _A_SCD_AEnrollees @date_in,@time_in,@year";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                var command = new SqlCommand(sqlText, conn);
                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@date_in", dateIn);
                command.Parameters.AddWithValue("@time_in", timeIn);
                var t = command.CommandText;

                try
                {
                    conn.Open();
                    result.Load(command.ExecuteReader());
                    conn.Close();
                } catch {
                    return null;
                }
                
            }

            return result;
        }

        private DataTable FetchDynamicConfig()
        {
            var result = new DataTable();
            var sqlText = "exec _A_SCD 'portal','2',0,0";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                var command = new SqlCommand(sqlText, conn);

                try
                {
                    conn.Open();
                    result.Load(command.ExecuteReader());
                    conn.Close();
                }
                catch {
                    return null;
                }
            }

            return result;
        }
        #endregion
    }
}
