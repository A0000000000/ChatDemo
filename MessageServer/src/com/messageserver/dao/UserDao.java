package com.messageserver.dao;

import com.messageserver.domain.User;

public interface UserDao extends BaseDao {

    User selectUserByUsername(String username) throws Exception;

    void insertUser(User user) throws Exception;

    void updateUser(User user) throws Exception;

    User selectUserById(String id) throws Exception;
}
