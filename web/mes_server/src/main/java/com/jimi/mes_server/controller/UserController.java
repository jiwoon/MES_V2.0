package com.jimi.mes_server.controller;

import com.jfinal.aop.Enhancer;
import com.jfinal.core.Controller;
import com.jfinal.core.paragetter.Para;
import com.jimi.mes_server.annotation.Access;
import com.jimi.mes_server.model.GpsUser;
import com.jimi.mes_server.service.UserService;
import com.jimi.mes_server.util.ResultUtil;

/**
 * 用户控制器
 * <br>
 * <b>2018年6月1日</b>
 * @author 沫熊工作室 <a href="http://www.darhao.cc">www.darhao.cc</a>
 */
public class UserController extends Controller  {

	private UserService userService = Enhancer.enhance(UserService.class);
	
	public static final String USER_TABLE_NAME = "Gps_User";
	
	public static final String SESSION_KEY_LOGIN_USER = "loginUser";
	
	
	public void select(String table, Integer pageNo, Integer pageSize, String ascBy, String descBy, String filter){
		renderJson(ResultUtil.succeed(userService.select(table, pageNo, pageSize, ascBy, descBy, filter)));
	}
	
	
	public void login(String userName, String password) {
		GpsUser user = userService.login(userName, password);
		setSessionAttr(SESSION_KEY_LOGIN_USER, user);
		renderJson(ResultUtil.succeed(user));
	}
	
	
	public void logout() {
		setSessionAttr(SESSION_KEY_LOGIN_USER, null);
		renderJson(ResultUtil.succeed());
	}
	
	
	public void checkLogined() {
		GpsUser user = getSessionAttr(SESSION_KEY_LOGIN_USER);
		if(user != null) {
			renderJson(ResultUtil.succeed(user));
		}else {
			renderJson(ResultUtil.succeed("no user signed in"));
		}
	}
	
	@Access({"SuperAdmin"})
	public void add(@Para("") GpsUser user) {
		if(userService.add(user)) {
			renderJson(ResultUtil.succeed());
		}else {
			renderJson(ResultUtil.failed());
		}
	}
	
	
	@Access({"SuperAdmin"})
	public void update(@Para("") GpsUser user) {
		if(userService.update(user)) {
			renderJson(ResultUtil.succeed());
		}else {
			renderJson(ResultUtil.failed());
		}
	}
}
