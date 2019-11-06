package com.messageserver.utils;

import com.messageserver.dao.BaseDao;

import java.util.ResourceBundle;

public class DaoFactory {

    private static ResourceBundle bundle = ResourceBundle.getBundle("Bean-Dao");

    public static BaseDao newDaoInstance(String name){
        BaseDao res = null;
        String beanName = bundle.getString(name);
        try {
            Class clazz = Class.forName(beanName);
            res = (BaseDao) clazz.getConstructor().newInstance();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return res;
    }

}
