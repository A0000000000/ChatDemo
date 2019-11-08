package com.messageserver.service.impl;

import com.messageserver.dao.FriendDao;
import com.messageserver.dao.MessageDao;
import com.messageserver.dao.UserDao;

public class BaseService {

    private FriendDao friendDao;
    private MessageDao messageDao;
    private UserDao userDao;

    public FriendDao getFriendDao() {
        return friendDao;
    }

    public void setFriendDao(FriendDao friendDao) {
        this.friendDao = friendDao;
    }

    public MessageDao getMessageDao() {
        return messageDao;
    }

    public void setMessageDao(MessageDao messageDao) {
        this.messageDao = messageDao;
    }

    public UserDao getUserDao() {
        return userDao;
    }

    public void setUserDao(UserDao userDao) {
        this.userDao = userDao;
    }
}
