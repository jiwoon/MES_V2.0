package com.jimi.mes_server.config;

import com.jfinal.config.Constants;
import com.jfinal.config.Handlers;
import com.jfinal.config.Interceptors;
import com.jfinal.config.JFinalConfig;
import com.jfinal.config.Plugins;
import com.jfinal.config.Routes;
import com.jfinal.kit.PropKit;
import com.jfinal.plugin.activerecord.ActiveRecordPlugin;
import com.jfinal.plugin.activerecord.dialect.SqlServerDialect;
import com.jfinal.plugin.activerecord.tx.Tx;
import com.jfinal.plugin.druid.DruidPlugin;
import com.jfinal.template.Engine;
import com.jimi.mes_server.controller.OrderController;
import com.jimi.mes_server.controller.ReportController;
import com.jimi.mes_server.controller.UserController;
import com.jimi.mes_server.interceptor.AccessInterceptor;
import com.jimi.mes_server.interceptor.CORSInterceptor;
import com.jimi.mes_server.interceptor.ErrorLogInterceptor;
import com.jimi.mes_server.model.MappingKit;

/**
 * 全局配置
 * <br>
 * <b>2018年5月22日</b>
 * @author 沫熊工作室 <a href="http://www.darhao.cc">www.darhao.cc</a>
 */
public class MesConfig extends JFinalConfig {
	
	
	@Override
	public void configConstant(Constants me) {
		me.setDevMode(true);
	}

	
	@Override
	public void configEngine(Engine me) {
	}

	
	@Override
	public void configHandler(Handlers me) {
	}

	
	@Override
	public void configInterceptor(Interceptors me) {
		me.addGlobalActionInterceptor(new ErrorLogInterceptor());
		me.addGlobalActionInterceptor(new CORSInterceptor());
		me.addGlobalActionInterceptor(new AccessInterceptor());
		me.addGlobalServiceInterceptor(new Tx());
	}

	@Override
	public void configPlugin(Plugins me) {
		PropKit.use("properties.ini");
		//配置数据连接池
		DruidPlugin dp = new DruidPlugin(PropKit.get("url"), PropKit.get("user"), PropKit.get("password"));
		me.add(dp);
		//配置ORM
	    ActiveRecordPlugin arp = new ActiveRecordPlugin(dp);
	    arp.setDialect(new SqlServerDialect());
	    arp.setShowSql(true);
	    MappingKit.mapping(arp);
	    me.add(arp);
	}

	
	@Override
	public void configRoute(Routes me) {
		me.add("/report", ReportController.class);
		me.add("/order", OrderController.class);
		me.add("/user", UserController.class);
	}
	
	
	@Override
	public void afterJFinalStart() {
		System.out.println("Mes Server is Running now...");
	}
	
	
	@Override
	public void beforeJFinalStop() {
	}
	
}
