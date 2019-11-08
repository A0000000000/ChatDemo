package com.messageserver.service.impl;

import com.messageserver.domain.User;
import com.messageserver.service.UserService;

public class UserServiceImpl extends BaseService implements UserService {

    @Override
    public User getUserByUsername(String username) {
        User user = null;
        try {
            user = getUserDao().selectUserByUsername(username);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return user;
    }

    @Override
    public boolean register(User user) {
        try {
            getUserDao().insert(user);
            return true;
        } catch (Exception e) {
            e.printStackTrace();
        }
        return false;
    }

    @Override
    public boolean changePassword(User user, String password) {
        user.setPassword(password);
        try {
            getUserDao().update(user);
            return true;
        } catch (Exception e) {
            e.printStackTrace();
        }
        return false;
    }

    @Override
    public User getUserById(String userId) {
        User user = null;
        try {
            user = getUserDao().find(userId);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return user;
    }

    @Override
    public boolean changeStatus(User user, int status) {
        user.setStatus(status);
        try {
            getUserDao().update(user);
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
    }
}
