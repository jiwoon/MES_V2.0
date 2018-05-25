package com.jimi.mes_server.interceptor;

import javax.servlet.http.HttpServletResponse;

import com.jfinal.aop.Interceptor;
import com.jfinal.aop.Invocation;


public class CorsInterceptor implements Interceptor {

	@Override
	public void intercept(Invocation invocation) {
		 HttpServletResponse response = invocation.getController().getResponse();
		 response.addHeader("Access-Control-Allow-Origin", "*");
		 response.setHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
		 response.setHeader("Access-Control-Allow-Methods", "GET, PUT, DELETE, POST");
		 invocation.invoke();
	}

}
