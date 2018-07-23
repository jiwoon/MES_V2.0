using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Print_Message
{
    public class Gps_ManuOrderParam
    {
        //制单号
        public string ZhiDan { get; set; }

        //软件机型
        public string SoftModel { get; set; }

        //SN固定前缀
        public string SN1 { get; set; }

        //SN2--机身贴后缀
        public string SN2 { get; set; }

        //SN3--彩盒贴后缀
        public string SN3 { get; set; }

        //盒子号1
        public string Box_No1 { get; set; }

        //盒子号2
        public string Box_No2 { get; set; }

        //生产日期
        public string ProductDate { get; set; }

        //颜色
        public string Color { get; set; }

        //重量
        public string Weight { get; set; }

        //Qty
        public string Qty { get; set; }

        //生产号
        public string ProductNo { get; set; }

        //版本
        public string Version { get; set; }

        //IMEI起始位
        public string IMEIStart { get; set; }

        //IMEI终止位
        public string IMEIEnd { get; set; }

        //SIM起始位
        public string SIMStart { get; set; }

        //SIM终止位
        public string SIMEnd { get; set; }

        //BAT起始位
        public string BATStart { get; set; }

        //BAT终止位
        public string BATEnd { get; set; }

        //VIP起始位
        public string VIPStart { get; set; }

        //VIP终止位
        public string VIPEnd { get; set; }

        //绑定关系
        public string IMEIRel { get; set; }

        //备注
        public string Remark1 { get; set; }

        //制单状态
        public int status { get; set; }

        //机身贴模板
        public string JST_template { get; set; }

        //彩盒贴模板1
        public string CHT_template1 { get; set; }

        //彩盒贴模板2
        public string CHT_template2 { get; set; }

        //BAT前缀
        public string BAT_prefix { get; set; }

        //BAT位数
        public string BAT_digits { get; set; }

        //SIM前缀
        public string SIM_prefix { get; set; }

        //SIM位数
        public string SIM_digits { get; set; }

        //VIP前缀
        public string VIP_prefix { get; set; }

        //VIP位数
        public string VIP_digits { get; set; }

        //ICCID前缀
        public string ICCID_prefix { get; set; }

        //ICCID位数
        public string ICCID_digits { get; set; }

        //IMEI当前打印位
        public string IMEIPrints { get; set; }
    }
}
