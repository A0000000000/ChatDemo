package com.messageserver.service.impl;

import com.messageserver.domain.Friend;
import com.messageserver.domain.User;
import com.messageserver.service.FriendService;

import java.util.List;

public class FriendServiceImpl extends BaseService implements FriendService {


    @Override
    public List<Friend> getAllFriend(User user) {
        List<Friend> res = null;
        try {
            res = getFriendDao().selectFriendByUser(user);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return res;
    }

    @Override
    public boolean saveFriend(Friend friend) {
        try {
            getFriendDao().insert(friend);
            return true;
        } catch (Exception e) {
            e.printStackTrace();
        }
        return false;
    }

    @Override
    public boolean saveChange(Friend friend) {
        try {
            getFriendDao().update(friend);
            return true;
        } catch (Exception e) {
            e.printStackTrace();
        }
        return false;
    }

    @Override
    public Friend getFriend(String masterId, String targetId) {
        Friend res = null;
        try {
            res = getFriendDao().selectFriend(masterId, targetId);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return res;
    }
}
