package com.jimi.mes_server.interceptor;

import com.jfinal.aop.Interceptor;
import com.jfinal.aop.Invocation;
import com.jimi.mes_server.annotation.Access;
import com.jimi.mes_server.controller.UserController;
import com.jimi.mes_server.exception.AccessException;
import com.jimi.mes_server.model.GpsUser;
import com.jimi.mes_server.util.TokenBox;


/**
 * 用户权限拦截器
 * <br>
 * <b>2018年5月29日</b>
 * @author 沫熊工作室 <a href="http://www.darhao.cc">www.darhao.cc</a>
 */
public class AccessInterceptor implements Interceptor {

	@Override
	public void intercept(Invocation invocation) {
		 Access access = invocation.getMethod().getAnnotation(Access.class);
		 if(access == null) {
			 invocation.invoke();
			 return;
		 }
		 String token = invocation.getController().getPara(TokenBox.TOKEN_ID_KEY_NAME);
		 GpsUser user = TokenBox.get(token, UserController.SESSION_KEY_LOGIN_USER);
		 if(user == null) {
			 throw new AccessException("not logined");
		 }
		 if(!user.getInService()) {
			 throw new AccessException("the user is disabled");
		 }
		 String[] accessUserTypes = access.value();
		 for (String userType : accessUserTypes) {
			if(userType.equals(user.getUserType())){
				invocation.invoke();
				return;
			}
		}
		throw new AccessException("access denied");
	}

}
