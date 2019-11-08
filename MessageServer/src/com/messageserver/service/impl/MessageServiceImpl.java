package com.messageserver.service.impl;

import com.messageserver.domain.Message;
import com.messageserver.service.MessageService;

import java.util.List;

public class MessageServiceImpl extends BaseService implements MessageService {


    @Override
    public boolean addNewMessage(Message message) {
        try {
            getMessageDao().insert(message);
            return true;
        } catch (Exception e) {
            e.printStackTrace();
        }
        return false;
    }

    @Override
    public List<Message> getNewMessageByReceiverId(String id) {
        List<Message> res = null;
        try {
            res = getMessageDao().selectNewMessageByReceiverId(id);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return res;
    }

    @Override
    public boolean saveChange(Message message) {
        try {
            getMessageDao().update(message);
            return true;
        } catch (Exception e) {
            e.printStackTrace();
        }
        return false;
    }
}
