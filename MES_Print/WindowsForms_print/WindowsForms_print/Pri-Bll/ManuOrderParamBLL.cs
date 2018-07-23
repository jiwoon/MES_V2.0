using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Print_Message;
using ManuOrder.Param.DAL;

namespace ManuOrder.Param.BLL
{
    public class ManuOrderParamBLL
    {
        ManuOrderParamDAL MOPD = new ManuOrderParamDAL();

        public List<Gps_ManuOrderParam> SelectZhidanNumBLL() {
            return MOPD.SelectZhidanNumDAL();
        }

        public List<Gps_ManuOrderParam> SelectZhidanNumBylikeBLL(string likeword)
        {
            return MOPD.SelectZhidanNumBylikeDAL(likeword);
        }

        public List<Gps_ManuOrderParam> selectManuOrderParamByzhidanBLL(string ZhidanNum) {
            return MOPD.selectManuOrderParamByzhidanDAL(ZhidanNum);
        }

        public bool UpdateSNnumberBLL(string ZhiDanNum, string SN1, string SN2, string SN3, string VIP1, string VIP2,long imeiPrints) {
            if (MOPD.UpdateSNnumberDAL(ZhiDanNum,SN1,SN2,SN3,VIP1,VIP2,imeiPrints)> 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateSN3numberBLL(string ZhiDanNum, string SN1, string SN2, string SN3,string VIP1,string VIP2)
        {
            if (MOPD.UpdateSN3numberDAL(ZhiDanNum, SN1,SN2, SN3,VIP1, VIP2) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateCHmesBLL(string IMEI, string CHPrintTime,string lj1,string lj2)
        {
            if (MOPD.UpdateCHmesDAL(IMEI, CHPrintTime, lj1, lj2) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateCHSIMBLL(string IMEI, string CHPrintTime, string lj1, string lj2 ,string SIM)
        {
            if (MOPD.UpdateCHSIMDAL(IMEI, CHPrintTime, lj1, lj2,SIM) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateCHVIPBLL(string IMEI, string CHPrintTime, string lj1, string lj2, string VIP)
        {
            if (MOPD.UpdateCHVIPDAL(IMEI, CHPrintTime, lj1, lj2, VIP) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateCHVIPAndSimBLL(string IMEI, string CHPrintTime, string lj1, string lj2, string VIP,string SIM)
        {
            if (MOPD.UpdateCHVIPAndSimDAL(IMEI, CHPrintTime, lj1, lj2, VIP,SIM) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateJSmesBLL(string IMEI, string JSPrintTime, string lj1)
        {
            if (MOPD.UpdateJSmesDAL(IMEI, JSPrintTime, lj1) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateStatusByZhiDanBLL(string ZhiDanNum)
        {
            MOPD.UpdateStatusByZhiDanDAL(ZhiDanNum);
        }


    }
}
