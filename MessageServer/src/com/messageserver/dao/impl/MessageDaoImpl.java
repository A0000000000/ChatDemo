package com.messageserver.dao.impl;

import com.messageserver.dao.MessageDao;
import com.messageserver.domain.Message;
import com.messageserver.utils.SessionThreadLocal;
import org.hibernate.Session;
import org.hibernate.query.Query;

import java.util.List;

public class MessageDaoImpl implements MessageDao {
    @Override
    public void insertMessage(Message message) throws Exception {
        Session session = SessionThreadLocal.openSession();
        session.save(message);
    }

    @Override
    public List<Message> selectNewMessageByReceiverId(String id) throws Exception {
        Session session = SessionThreadLocal.openSession();
        Query query = session.createQuery("from com.messageserver.domain.Message as m where m.receiverId = :id and m.status = :status");
        query.setParameter("id", id);
        query.setParameter("status", 1);
        List<Message> list = query.list();
        return list;
    }

    @Override
    public void updateChange(Message message) throws Exception {
        Session session = SessionThreadLocal.openSession();
        session.update(message);
    }
}
