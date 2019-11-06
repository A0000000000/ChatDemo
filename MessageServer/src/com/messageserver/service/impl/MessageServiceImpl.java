package com.messageserver.service.impl;

import com.messageserver.dao.MessageDao;
import com.messageserver.domain.Message;
import com.messageserver.service.MessageService;
import com.messageserver.utils.DaoFactory;
import com.messageserver.utils.SessionThreadLocal;
import org.hibernate.Transaction;

import java.util.List;

public class MessageServiceImpl implements MessageService {

    private MessageDao messageDao = (MessageDao) DaoFactory.newDaoInstance("MessageDao");

    @Override
    public boolean addNewMessage(Message message) {
        Transaction transaction = SessionThreadLocal.openSession().beginTransaction();
        try {
            messageDao.insertMessage(message);
            transaction.commit();
            return true;
        } catch (Exception e) {
            e.printStackTrace();
        }
        transaction.rollback();
        return false;
    }

    @Override
    public List<Message> getNewMessageByReceiverId(String id) {
        List<Message> res = null;
        Transaction transaction = SessionThreadLocal.openSession().beginTransaction();
        try {
            res = messageDao.selectNewMessageByReceiverId(id);
            transaction.commit();
        } catch (Exception e) {
            e.printStackTrace();
            transaction.rollback();
        }
        return res;
    }

    @Override
    public boolean saveChange(Message message) {
        Transaction transaction = SessionThreadLocal.openSession().beginTransaction();
        try {
            messageDao.updateChange(message);
            transaction.commit();
            return true;
        } catch (Exception e) {
            e.printStackTrace();
        }
        transaction.rollback();
        return false;
    }
}
