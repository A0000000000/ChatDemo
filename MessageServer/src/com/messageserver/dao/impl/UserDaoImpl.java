package com.messageserver.dao.impl;

import com.messageserver.dao.UserDao;
import com.messageserver.domain.User;
import com.messageserver.utils.SessionThreadLocal;
import org.hibernate.Session;
import org.hibernate.query.Query;

public class UserDaoImpl implements UserDao {
    @Override
    public User selectUserByUsername(String username) throws Exception {
        Session session = SessionThreadLocal.openSession();
        Query query = session.createQuery("from com.messageserver.domain.User as u where u.username = :username");
        query.setParameter("username", username);
        User user = (User) query.uniqueResult();
        return user;
    }

    @Override
    public void insertUser(User user) throws Exception {
        Session session = SessionThreadLocal.openSession();
        session.save(user);
    }

    @Override
    public void updateUser(User user) throws Exception {
        Session session = SessionThreadLocal.openSession();
        session.update(user);
    }

    @Override
    public User selectUserById(String id) throws Exception {
        Session session = SessionThreadLocal.openSession();
        return session.get(User.class, id);
    }
}
