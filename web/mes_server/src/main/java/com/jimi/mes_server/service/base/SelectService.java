package com.jimi.mes_server.service.base;

import java.util.ArrayList;
import java.util.List;

import com.jfinal.kit.PropKit;
import com.jfinal.plugin.activerecord.Db;
import com.jfinal.plugin.activerecord.Page;
import com.jfinal.plugin.activerecord.Record;

/**
 * 通用查询业务层
 * <br>
 * <b>2018年5月23日</b>
 * @author 沫熊工作室 <a href="http://www.darhao.cc">www.darhao.cc</a>
 */
public class SelectService {

	/**
	 * 分页查询，支持筛选和排序
	 * @param table 提供可读的表名
	 * @param pageNo 页码，从1开始
	 * @param pageSize 每页的条目数
	 * @param ascBy 按指定字段升序，不可和descBy同时使用
	 * @param descBy 按指定字段降序，不可和ascBy同时使用
	 * @param filter 按字段筛选，支持<, >, >,=, <=, !=, =，多个字段请用&隔开
	 * @return Page对象
	 */
	public Page<Record> select(String table, Integer pageNo, Integer pageSize, String ascBy, String descBy, String filter){
		StringBuffer sql = new StringBuffer();
		List<String> questionValues = new ArrayList<>();
		createFrom(table, sql);
		createWhere(filter, questionValues, sql);
		createOrderBy(ascBy, descBy, sql);
		return paginateAndFillWhereValues(pageNo, pageSize, sql, questionValues);
	}
	
	
	private void createFrom(String table, StringBuffer sql) {
		//表名非空判断
		if(table == null) {
			throw new RuntimeException("table name must be provided");
		}
		//表是否在可读范围内
		String[] reportTables = PropKit.use("properties.ini").get("readableTables").split(",");
		for (String tablePara : reportTables) {
			//按冒号分割表名和主键名（多个主键用&隔开）
			if(tablePara.equals(table)) {
				sql.append(" FROM " + table);
				return;
			}
		}
		throw new RuntimeException("not a readable table");
	}

	
	private void createWhere(String filter, List<String> questionValues, StringBuffer sql) {
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
				questionValues.add(value);
				if(index == whereUnits.length - 1) {
					sql.delete(sql.lastIndexOf("AND"), sql.length());
				}
				index++;
			}
		}
	}


	private void createOrderBy(String ascBy, String descBy, StringBuffer sql) {
		if(ascBy != null && descBy != null) {
			throw new RuntimeException("ascBy and descBy can not be provided at the same time");
		}else if(ascBy != null) {
			sql.append(" ORDER BY " + ascBy + " ASC ");
		}else if(descBy != null){
			sql.append(" ORDER BY " + descBy + " DESC ");
		}
	}


	private Page<Record> paginateAndFillWhereValues(Integer pageNo, Integer pageSize, StringBuffer sql, List<String> questionValues) {
		if((pageNo != null && pageSize == null) || (pageNo == null && pageSize != null)) {
			throw new RuntimeException("ascBy and descBy must be provided at the same time");
		}
		if(pageNo == null && pageSize == null) {
			return Db.paginate(1, PropKit.use("properties.ini").getInt("defaultPageSize"), "SELECT *", sql.toString(), questionValues.toArray());
		}else {
			return Db.paginate(pageNo, pageSize, "SELECT *", sql.toString(), questionValues.toArray());
		}
	}
	
}
