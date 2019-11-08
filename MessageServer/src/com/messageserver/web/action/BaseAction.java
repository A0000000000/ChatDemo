package com.messageserver.web.action;

import com.messageserver.service.FriendService;
import com.messageserver.service.MessageService;
import com.messageserver.service.UserService;
import com.opensymphony.xwork2.ActionSupport;
import com.opensymphony.xwork2.ModelDriven;

import java.lang.reflect.ParameterizedType;

public class BaseAction<T> extends ActionSupport implements ModelDriven<T> {

    private T bean;

    private FriendService friendService;
    private MessageService messageService;
    private UserService userService;

    public BaseAction() {
        Class<T> beanClass = (Class<T>) ((ParameterizedType) this.getClass().getGenericSuperclass()).getActualTypeArguments()[0];
        try {
            bean = beanClass.getConstructor().newInstance();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }


    public T getBean() {
        return bean;
    }

    public void setBean(T bean) {
        this.bean = bean;
    }


    public FriendService getFriendService() {
        return friendService;
    }

    public void setFriendService(FriendService friendService) {
        this.friendService = friendService;
    }

    public MessageService getMessageService() {
        return messageService;
    }

    public void setMessageService(MessageService messageService) {
        this.messageService = messageService;
    }

    public UserService getUserService() {
        return userService;
    }

    public void setUserService(UserService userService) {
        this.userService = userService;
    }

    @Override
    public T getModel() {
        return bean;
    }
}
