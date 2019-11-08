package com.messageserver.dao;

import com.messageserver.domain.Message;

import java.util.List;

public interface MessageDao extends BaseDao<Message> {

    List<Message> selectNewMessageByReceiverId(String id) throws Exception;

}
