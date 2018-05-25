package com.jimi.mes_server.controller;

import java.util.ArrayList;
import java.util.List;

import org.apache.log4j.LogManager;
import org.apache.log4j.Logger;

import com.jfinal.core.Controller;
import com.jfinal.kit.PropKit;
import com.jfinal.plugin.activerecord.Db;
import com.jfinal.plugin.activerecord.Page;
import com.jfinal.plugin.activerecord.Record;
import com.jimi.mes_server.util.ResultUtil;

/**
 * 报表控制器
 * 支持表选择、分页、排序、参数筛选（需要前缀p_）
 * <br>
 * <b>2018年5月23日</b>
 * @author 沫熊工作室 <a href="http://www.darhao.cc">www.darhao.cc</a>
 */
public class ReportController extends Controller {
	
	private static Logger logger = LogManager.getRootLogger();
	
	private List<String> whereParametersValues = new ArrayList<>();
	private StringBuffer sql = new StringBuffer();
	private Page<Record> records;
	
	
	public void select(String table, Integer pageNo, Integer pageSize, String ascBy, String descBy, String filter) {
		try {
			createFrom(table);
			createWhere(filter);
			createOrderBy(ascBy, descBy);
			paginateAndFillWhereValues(pageNo, pageSize);
			renderJson(ResultUtil.succeed(records));
		} catch (RuntimeException e) {
			e.printStackTrace();
			logger.error(e.getMessage());
			renderJson(ResultUtil.failed(e.getMessage()));
		}
	}
	

	private void createFrom(String table) {
		//表名非空判断
		if(table == null) {
			throw new RuntimeException("table name must be provided");
		}
		//表是否在范围内
		String[] reportTables = PropKit.use("properties.ini").get("reportTables").split(",");
		for (String tableName : reportTables) {
			if(tableName.equals(table)) {
				sql.append(" FROM " + table);
				return;
			}
		}
		throw new RuntimeException("not a report table");
	}

	
	private void createWhere(String filter) {
		//判断filter存在与否
		if(filter != null) {
			sql.append(" WHERE ");
			String[] whereUnits = filter.split("&");
			int index = 0;
			for (String whereUnit: whereUnits) {
				//分割键值与运算符
				int operatorStartIndex = -1;
				StringBuffer operator = new StringBuffer();
				for (int i = 0; i < whereUnit.length(); i++) {
					char c = whereUnit.charAt(i);
					if(c == '>' || c == '<' || c == '=' || c == '!') {
						operator.append(c);
						if(operatorStartIndex == -1) {
							operatorStartIndex = i;
						}
					}
				}
				String key = whereUnit.substring(0, operatorStartIndex);
				String value = whereUnit.substring(operatorStartIndex + operator.length(), whereUnit.length());
				sql.append(key + operator.toString() +"? AND ");
				whereParametersValues.add(value);
				if(index == whereUnits.length - 1) {
					sql.delete(sql.lastIndexOf("AND"), sql.length());
				}
				index++;
			}
		}
	}


	
	
	private void createOrderBy(String ascBy, String descBy) {
		if(ascBy != null && descBy != null) {
			throw new RuntimeException("ascBy and descBy can not be provided at the same time");
		}else if(ascBy != null) {
			sql.append(" ORDER BY " + ascBy + " ASC ");
		}else if(descBy != null){
			sql.append(" ORDER BY " + descBy + " DESC ");
		}
	}


	private void paginateAndFillWhereValues(Integer pageNo, Integer pageSize) {
		if((pageNo != null && pageSize == null) || (pageNo == null && pageSize != null)) {
			throw new RuntimeException("ascBy and descBy must be provided at the same time");
		}
		if(pageNo == null && pageSize == null) {
			records = Db.paginate(1, PropKit.use("properties.ini").getInt("defaultPageSize"), "SELECT *", sql.toString(), whereParametersValues.toArray());
		}else {
			records = Db.paginate(pageNo, pageSize, "SELECT *", sql.toString(), whereParametersValues.toArray());
		}
	}

}
