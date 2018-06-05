package com.jimi.mes_server.interceptor;

import java.util.Map;

import com.jfinal.aop.Interceptor;
import com.jfinal.aop.Invocation;
import com.jimi.mes_server.exception.OperationException;


/**
 * 空值拦截器
 * <br>
 * <b>2018年5月29日</b>
 * @author 沫熊工作室 <a href="http://www.darhao.cc">www.darhao.cc</a>
 */
public class NullValueInterceptor implements Interceptor {

	@Override
	public void intercept(Invocation invocation) {
		Map<String, String[]> parameters = invocation.getController().getParaMap();
		for (String[] para : parameters.values()) {
			if(para == null || para.length == 0 || para[0] == "") {
				throw new OperationException("parameters can not be empty value");
			}
		}
		invocation.invoke();
	}

}
