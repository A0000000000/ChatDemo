package com.messageserver.dao.impl;

import com.messageserver.dao.BaseDao;
import org.springframework.orm.hibernate5.support.HibernateDaoSupport;

import java.io.Serializable;
import java.lang.reflect.ParameterizedType;

public class BaseDaoImpl<T> extends HibernateDaoSupport implements BaseDao<T> {
    @Override
    public void insert(T t) throws Exception{
        getHibernateTemplate().save(t);
    }

    @Override
    public void update(T t) throws Exception{
        getHibernateTemplate().update(t);
    }

    @Override
    public void delete(T t) throws Exception{
        getHibernateTemplate().delete(t);
    }

    @Override
    public T find(Serializable id) throws Exception{
        return getHibernateTemplate().get((Class<T>)((ParameterizedType)this.getClass().getGenericSuperclass()).getActualTypeArguments()[0], id);
    }
}
