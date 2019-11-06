package com.messageserver.service;

import com.messageserver.domain.User;

public interface UserService extends BaseService {

    User getUserByUsername(String username);

    boolean register(User user);

    boolean changePassword(User user, String password);

    User getUserById(String userId);

    boolean changeStatus(User user, int status);
}
