package com.messageserver.utils;

import com.messageserver.service.BaseService;

import java.util.ResourceBundle;

public class ServiceFactory {

    private static ResourceBundle bundle = ResourceBundle.getBundle("Bean-Service");

    public static BaseService newServiceInstance(String name){
        BaseService res = null;
        String beanName = bundle.getString(name);
        try {
            Class clazz = Class.forName(beanName);
            res = (BaseService) clazz.getConstructor().newInstance();
        }catch (Exception e){
            e.printStackTrace();
        }
        return res;
    }

}
