package com.messageserver.dao.impl;

import com.messageserver.dao.MessageDao;
import com.messageserver.domain.Message;

import java.util.List;

public class MessageDaoImpl extends BaseDaoImpl<Message> implements MessageDao {

    @Override
    public List<Message> selectNewMessageByReceiverId(String id) throws Exception {
        Message example = new Message();
        example.setReceiverId(id);
        return getHibernateTemplate().findByExample(example);
    }

}
