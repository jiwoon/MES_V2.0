using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Print_Message;
using Print.Message.Dal;

namespace Print.Message.Bll
{
    public class PrintMessageBLL
    {
        public bool InsertPrintMessageBLL(List<PrintMessage> list) {
            PrintMessageDAL PMD = new PrintMessageDAL();
            if (PMD.InsertPrintMessageDAL(list) > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public bool CheckCHOrJSIMEIBLL(string IMEInumber, int PrintType)
        {
            PrintMessageDAL PMD = new PrintMessageDAL();
            if (PMD.CheckCHOrJSIMEIDAL(IMEInumber,PrintType) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckReCHOrJSIMEIBLL(string IMEInumber, int PrintType)
        {
            PrintMessageDAL PMD = new PrintMessageDAL();
            if (PMD.CheckReCHOrJSIMEIDAL(IMEInumber, PrintType) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckIMEIBLL(string IMEInumber)
        {
            PrintMessageDAL PMD = new PrintMessageDAL();
            if (PMD.CheckIMEIDAL(IMEInumber) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckSIMBLL(string SIM)
        {
            PrintMessageDAL PMD = new PrintMessageDAL();
            if (PMD.CheckSIMDAL(SIM) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckVIPBLL(string VIP)
        {
            PrintMessageDAL PMD = new PrintMessageDAL();
            if (PMD.CheckVIPDAL(VIP) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<PrintMessage> SelectSnByIMEIBLL(string IMEInumber) {
            PrintMessageDAL PMD = new PrintMessageDAL();
            return PMD.SelectSnByIMEIDAL(IMEInumber);
        }

        public bool UpdateRePrintBLL(string IMEInumber,string RePrintTime,int PrintType,string lj,string lj1)
        {
            PrintMessageDAL PMD = new PrintMessageDAL();
            if (PrintType == 1)
            {
                if (PMD.SelectJS_RePrintNumByIMEIDAL(IMEInumber) == 0)
                {
                    if (PMD.UpdateRePrintDAL(IMEInumber, RePrintTime, lj) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (PMD.UpdateReEndPrintDAL(IMEInumber, RePrintTime, lj) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (PMD.SelectCH_RePrintNumByIMEIDAL(IMEInumber) == 0)
                {
                    if (PMD.UpdateCHRePrintDAL(IMEInumber, RePrintTime, lj,lj1) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (PMD.UpdateCHReEndPrintDAL(IMEInumber, RePrintTime, lj,lj1) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        //public bool UpdateRePrintByljBLL(string IMEInumber, string RePrintTime, int PrintType,string path)
        //{
        //    PrintMessageDAL PMD = new PrintMessageDAL();
        //    if (PMD.SelectRePrintNumByIMEIAndPathDAL(IMEInumber,path) == 0)
        //    {
        //        if (PMD.UpdateRePrintByljDAL(IMEInumber, RePrintTime, PrintType,path) > 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        if (PMD.UpdateReEndPrintByljDAL(IMEInumber, RePrintTime, PrintType,path) > 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        public List<PrintMessage> SelectAllReJSTBLL()
        {
            PrintMessageDAL PMD = new PrintMessageDAL();
            return PMD.SelectAllReJSTDAL();
        }
        public List<PrintMessage> SelectAllReCHTBLL()
        {
            PrintMessageDAL PMD = new PrintMessageDAL();
            return PMD.SelectAllReCHTDAL();
        }

        public List<PrintMessage> SelectReMesByZhiDanOrIMEIBLL(string InputNum)
        {
            PrintMessageDAL PMD = new PrintMessageDAL();
            return PMD.SelectReMesByZhiDanOrIMEIDAL(InputNum);
        }

        public List<PrintMessage> SelectPrintMesBySNOrIMEIBLL(string conditions)
        {
            PrintMessageDAL PMD = new PrintMessageDAL();
            return PMD.SelectPrintMesBySNOrIMEIDAL(conditions);
        }

        public bool DeletePrintMessageBLL(int ID,string field)
        {
            PrintMessageDAL PMD = new PrintMessageDAL();
            if (PMD.DeletePrintMessageDAL(ID,field) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //public List<PrintMessage> SelectPrintMesByIdBLL(string id)
        //{
        //    PrintMessageDAL PMD = new PrintMessageDAL();
        //    return PMD.SelectPrintMesByIdDAL(id);
        //}


    }
}
