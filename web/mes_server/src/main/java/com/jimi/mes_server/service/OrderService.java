package com.jimi.mes_server.service;

import com.jfinal.aop.Enhancer;
import com.jfinal.plugin.activerecord.Db;
import com.jfinal.plugin.activerecord.Record;
import com.jimi.mes_server.controller.OrderController;
import com.jimi.mes_server.model.GpsManuorderparam;

/**
 * 订单业务层
 * <br>
 * <b>2018年5月29日</b>
 * @author 沫熊工作室 <a href="http://www.darhao.cc">www.darhao.cc</a>
 */
public class OrderService {
	
	private static DAOService daoService = Enhancer.enhance(DAOService.class);

	
	public boolean update(String key, String kv) {
		GpsManuorderparam order = GpsManuorderparam.dao.findById(key);
		if(order.getStatus() != 0) {
			throw new RuntimeException("only update the order in the non start state");
		}
		return daoService.update(OrderController.ORDER_TABLE_NAME, key, kv) == 1;
	}
	
	
	public boolean create(GpsManuorderparam order) {
		order.setStatus(0);
		return order.save();
	}
	
	
	public boolean copy(String key) {
		Record order = Db.findById(OrderController.ORDER_TABLE_NAME, key);
		Record newOrder = new Record();
		String[] exceptCols = new String[] {"Id", "SIMStart", "SIMEnd", "BATStart", "VIPStart", "VIPEnd", "BATEnd", "_MASK_FROM_V2"};
		for (String col : order.getColumnNames()) {
			boolean skip = false;
			for (String exceptCol : exceptCols) {
				if(col.equals(exceptCol)) {
					skip = true;
					break;
				}
			}
			if(skip) {
				continue;
			}
			newOrder.set(col, order.get(col));
		}
		newOrder
			.set("Status", 0)
			.set("IMEIStart", "00000000000000")
			.set("IMEIEnd", "00000000000000");
		return Db.save(OrderController.ORDER_TABLE_NAME, newOrder);
	}


	public boolean start(String key) {
		GpsManuorderparam order = GpsManuorderparam.dao.findById(key);
		if(order.getStatus() == 0){
			order.setStatus(1);
			return order.update();
		}else {
			throw new RuntimeException("only start the order in non start state");
		}
	}
	
	
	public boolean finish(String key) {
		GpsManuorderparam order = GpsManuorderparam.dao.findById(key);
		if(order.getStatus() <= 1){
			order.setStatus(2);
			return order.update();
		}else {
			throw new RuntimeException("only finish the order in non-start or start state");
		}
	}
	
	
	public boolean cancel(String key) {
		GpsManuorderparam order = GpsManuorderparam.dao.findById(key);
		if(order.getStatus() <= 1){
			order.setStatus(3);
			return order.update();
		}else {
			throw new RuntimeException("only cancel the order in non-start or start state");
		}
	}
}
