package com.jimi.mes_server.exception;

/**
 * 请求的参数异常，result：400
 * <br>
 * <b>2018年6月2日</b>
 * @author 沫熊工作室 <a href="http://www.darhao.cc">www.darhao.cc</a>
 */
public class ParameterException extends RuntimeException {

	public ParameterException(String message) {
		super(message);
	}
	
}
