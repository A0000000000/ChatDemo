package com.messageserver.service.impl;

import com.messageserver.dao.UserDao;
import com.messageserver.domain.User;
import com.messageserver.service.UserService;
import com.messageserver.utils.DaoFactory;
import com.messageserver.utils.SessionThreadLocal;
import org.hibernate.Transaction;

public class UserServiceImpl implements UserService {

    private UserDao userDao = (UserDao) DaoFactory.newDaoInstance("UserDao");

    @Override
    public User getUserByUsername(String username) {
        Transaction transaction = SessionThreadLocal.openSession().beginTransaction();
        User user = null;
        try {
            user = userDao.selectUserByUsername(username);
            transaction.commit();
        } catch (Exception e) {
            e.printStackTrace();
            transaction.rollback();
        }
        return user;
    }

    @Override
    public boolean register(User user) {
        Transaction transaction = SessionThreadLocal.openSession().beginTransaction();
        try {
            userDao.insertUser(user);
            transaction.commit();
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            transaction.rollback();
        }
        return false;
    }

    @Override
    public boolean changePassword(User user, String password) {
        Transaction transaction = SessionThreadLocal.openSession().beginTransaction();
        user.setPassword(password);
        try {
            userDao.updateUser(user);
            transaction.commit();
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            transaction.rollback();
        }
        return false;
    }

    @Override
    public User getUserById(String userId) {
        User user = null;
        Transaction transaction = SessionThreadLocal.openSession().beginTransaction();
        try {
            user = userDao.selectUserById(userId);
            transaction.commit();
        } catch (Exception e) {
            e.printStackTrace();
            transaction.rollback();
        }
        return user;
    }

    @Override
    public boolean changeStatus(User user, int status) {
        Transaction transaction = SessionThreadLocal.openSession().beginTransaction();
        user.setStatus(status);
        try {
            userDao.updateUser(user);
            transaction.commit();
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            transaction.rollback();
            return false;
        }
    }
}
