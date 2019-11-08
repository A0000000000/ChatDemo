package com.messageserver.dao;

import com.messageserver.domain.User;

public interface UserDao extends BaseDao<User> {

    User selectUserByUsername(String username) throws Exception;

}
