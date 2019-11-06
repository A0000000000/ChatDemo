package com.messageserver.dao.impl;

import com.messageserver.dao.FriendDao;
import com.messageserver.domain.Friend;
import com.messageserver.domain.User;
import com.messageserver.utils.SessionThreadLocal;
import org.hibernate.Session;
import org.hibernate.query.Query;

import java.util.List;

public class FriendDaoImpl implements FriendDao {
    @Override
    public List<Friend> selectFriendByUser(User user) throws Exception {
        Session session = SessionThreadLocal.openSession();
        Query query = session.createQuery("from com.messageserver.domain.Friend as f where f.masterId = :masterId");
        query.setParameter("masterId", user.getId());
        List<Friend> list = query.list();
        return list;
    }

    @Override
    public void insertFriend(Friend friend) throws Exception {
        Session session = SessionThreadLocal.openSession();
        session.save(friend);
    }

    @Override
    public void update(Friend friend) throws Exception {
        Session session = SessionThreadLocal.openSession();
        session.update(friend);
    }

    @Override
    public Friend selectFriend(String masterId, String targetId) throws Exception {
        Session session = SessionThreadLocal.openSession();
        Query query = session.createQuery("from com.messageserver.domain.Friend as f where f.masterId = :masterId and f.friendId = :friendId");
        query.setParameter("masterId", masterId);
        query.setParameter("friendId", targetId);
        Friend res = (Friend) query.uniqueResult();
        return res;
    }
}
