package com.messageserver.dao;

import java.io.Serializable;

public interface BaseDao<T> {

    void insert(T t) throws Exception;
    void update(T t) throws Exception;
    void delete(T t) throws Exception;
    T find(Serializable id) throws Exception;

}
