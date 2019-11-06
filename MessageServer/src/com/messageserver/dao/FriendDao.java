package com.messageserver.dao;

import com.messageserver.domain.Friend;
import com.messageserver.domain.User;

import java.util.List;

public interface FriendDao extends BaseDao {

    List<Friend> selectFriendByUser(User user) throws Exception;

    void insertFriend(Friend friend) throws Exception;

    void update(Friend friend) throws Exception;

    Friend selectFriend(String masterId, String targetId) throws Exception;
}
