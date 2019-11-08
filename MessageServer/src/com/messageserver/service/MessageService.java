package com.messageserver.service;

import com.messageserver.domain.Message;

import java.util.List;

public interface MessageService {

    boolean addNewMessage(Message message);

    List<Message> getNewMessageByReceiverId(String id);

    boolean saveChange(Message message);
}
