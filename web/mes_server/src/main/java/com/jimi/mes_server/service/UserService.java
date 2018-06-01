package com.jimi.mes_server.service;

import java.util.Date;

import com.jimi.mes_server.model.GpsTestplan;
import com.jimi.mes_server.model.GpsUser;
import com.jimi.mes_server.model.GpsUsertype;
import com.jimi.mes_server.service.base.SelectService;

import cc.darhao.dautils.api.MD5Util;

/**
 * 用户业务层
 * <br>
 * <b>2018年5月29日</b>
 * @author 沫熊工作室 <a href="http://www.darhao.cc">www.darhao.cc</a>
 */
public class UserService extends SelectService{
	
	private static final String loginSql = "SELECT * FROM Gps_User WHERE UserName = ? AND UserPwd = ?";
	private static final String userTypeSql = "SELECT * FROM Gps_UserType WHERE  TypeName = ?";
	private static final String uniqueCheckSql = "SELECT * FROM Gps_User WHERE UserName = ?";
	
	public GpsUser login(String userName, String password) {
		GpsUser user = GpsUser.dao.findFirst(loginSql, userName, MD5Util.MD5(password));
		if(user == null) {
			throw new RuntimeException("userName or password is not correct");
		}
		if(!user.getInService()) {
			throw new RuntimeException("this user is disabled");
		}
		user.setLoginTime(new Date());
		user.update();
		return user;
	}
	
	
	public boolean add(GpsUser user) {
		checkUserTypeAndTestPlan(user);
		if(GpsUser.dao.find(uniqueCheckSql, user.getUserName()).size() != 0) {
			throw new RuntimeException("user is already exist");
		}
		user.keep("UserName","UserDes","UserPwd","UserType","UserTestPlan");
		user.setUserPwd(MD5Util.MD5(user.getUserPwd()));
		return user.save();
	}
	
	
	public boolean update(GpsUser user) {
		checkUserTypeAndTestPlan(user);
		user.keep("UserId","UserDes","UserType","UserTestPlan","InService");
		return user.update();
	}


	private void checkUserTypeAndTestPlan(GpsUser user) {
		String userType =  user.getUserType();
		if(GpsUsertype.dao.find(userTypeSql, userType).size() == 0) {
			throw new RuntimeException("user type not found");
		}
		String userTestPlan = user.getUserTestPlan();
		if(GpsTestplan.dao.findById(userTestPlan) == null) {
			throw new RuntimeException("user test plan not found");
		}
	}
}
