package com.messageserver.dao.impl;

import com.messageserver.dao.UserDao;
import com.messageserver.domain.User;

import java.util.List;

public class UserDaoImpl extends BaseDaoImpl<User> implements UserDao {
    @Override
    public User selectUserByUsername(String username) throws Exception {
        User example = new User();
        example.setUsername(username);
        List<User> list = getHibernateTemplate().findByExample(example);
        return list != null && list.size() > 0 ? list.get(0) : null;
    }

}
