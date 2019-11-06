package com.messageserver.utils;

import org.hibernate.Session;

public class SessionThreadLocal {

    public static Session openSession(){
        if(threadLocal.get() == null){
            threadLocal.set(HibernateUtils.openSession());
        }
        return threadLocal.get();
    }

    public static void closeSession(){
        if(threadLocal.get() != null){
            Session session = threadLocal.get();
            HibernateUtils.closeSession(session);
            threadLocal.remove();
        }
    }

    private static ThreadLocal<Session> threadLocal = new ThreadLocal<>();
}
