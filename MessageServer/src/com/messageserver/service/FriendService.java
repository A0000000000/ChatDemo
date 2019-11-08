package com.messageserver.service;

import com.messageserver.domain.Friend;
import com.messageserver.domain.User;

import java.util.List;

public interface FriendService {

    List<Friend> getAllFriend(User user);

    boolean saveFriend(Friend friend);

    boolean saveChange(Friend friend);

    Friend getFriend(String masterId, String targetId);
}
