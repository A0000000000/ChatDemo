package com.messageserver.dao.impl;

import com.messageserver.dao.FriendDao;
import com.messageserver.domain.Friend;
import com.messageserver.domain.User;

import java.util.List;

public class FriendDaoImpl extends BaseDaoImpl<Friend> implements FriendDao {
    @Override
    public List<Friend> selectFriendByUser(User user) throws Exception {
        Friend example = new Friend();
        example.setMasterId(user.getId());
        return getHibernateTemplate().findByExample(example);
    }

    @Override
    public Friend selectFriend(String masterId, String targetId) throws Exception {
        Friend example = new Friend();
        example.setMasterId(masterId);
        example.setFriendId(targetId);
        List<Friend> list = getHibernateTemplate().findByExample(example);
        return list != null && list.size() > 0 ? list.get(0) : null;
    }
}
