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
import com.jfinal.plugin.druid.DruidPlugin;
import com.jfinal.template.Engine;
import com.jimi.mes_server.controller.OrderController;
import com.jimi.mes_server.controller.ReportController;
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
//		//自动把Controller包下的所有控制器类名字的前缀的首字母小写化后作为Key
//		PropKit.use("properties.ini");
//		List<Class> controllerClasses = ClassScanner.searchClass(PropKit.get("controllerPackage"));
//		for (Class controllerClass : controllerClasses) {
//			String name = controllerClass.getSimpleName();
//			name = name.replaceAll("Controller", "");
//			name = name.substring(0, 1).toLowerCase() + name.substring(1, name.length());
//			me.add("/" + name, controllerClass);
//		}
		me.add("/report", ReportController.class);
		me.add("/order", OrderController.class);
	}
	
	
	@Override
	public void afterJFinalStart() {
		System.out.println("Mes Server is Running now...");
	}
	
	
	@Override
	public void beforeJFinalStop() {
	}
	

//	public static void main(String[] args){
//	    JFinal.start("src/main/webapp", 80, "/", 5);
//	}

}
