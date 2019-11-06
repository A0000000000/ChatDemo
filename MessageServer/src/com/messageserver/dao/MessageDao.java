package com.messageserver.dao;

import com.messageserver.domain.Message;

import java.util.List;

public interface MessageDao extends BaseDao {

    void insertMessage(Message message) throws Exception;

    List<Message> selectNewMessageByReceiverId(String id) throws Exception;

    void updateChange(Message message) throws Exception;
}
