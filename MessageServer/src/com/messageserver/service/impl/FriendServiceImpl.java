package com.messageserver.service.impl;

import com.messageserver.dao.FriendDao;
import com.messageserver.domain.Friend;
import com.messageserver.domain.User;
import com.messageserver.service.FriendService;
import com.messageserver.utils.DaoFactory;
import com.messageserver.utils.SessionThreadLocal;
import org.hibernate.Transaction;

import java.util.List;

public class FriendServiceImpl implements FriendService {

    private FriendDao friendDao = (FriendDao) DaoFactory.newDaoInstance("FriendDao");

    @Override
    public List<Friend> getAllFriend(User user) {
        List<Friend> res = null;
        Transaction transaction = SessionThreadLocal.openSession().beginTransaction();
        try {
            res = friendDao.selectFriendByUser(user);
            transaction.commit();
        } catch (Exception e) {
            e.printStackTrace();
            transaction.rollback();
        }
        return res;
    }

    @Override
    public boolean saveFriend(Friend friend) {
        Transaction transaction = SessionThreadLocal.openSession().beginTransaction();
        try {
            friendDao.insertFriend(friend);
            transaction.commit();
            return true;
        } catch (Exception e) {
            e.printStackTrace();
        }
        transaction.rollback();
        return false;
    }

    @Override
    public boolean saveChange(Friend friend) {
        Transaction transaction = SessionThreadLocal.openSession().beginTransaction();
        try {
            friendDao.update(friend);
            transaction.commit();
            return true;
        } catch (Exception e) {
            e.printStackTrace();
        }
        transaction.rollback();
        return false;
    }

    @Override
    public Friend getFriend(String masterId, String targetId) {
        Friend res = null;
        Transaction transaction = SessionThreadLocal.openSession().beginTransaction();
        try {
            res = friendDao.selectFriend(masterId, targetId);
            transaction.commit();
        } catch (Exception e) {
            e.printStackTrace();
            transaction.rollback();
        }
        return res;
    }
}
