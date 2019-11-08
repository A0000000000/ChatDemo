package com.messageserver.web.action;

import com.messageserver.domain.User;
import org.apache.struts2.ServletActionContext;

import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import java.io.IOException;

public class UserAction extends BaseAction<User> {

    public String checkUsername() throws IOException {
        User user = getUserService().getUserByUsername(getBean().getUsername());
        HttpServletResponse response = ServletActionContext.getResponse();
        if(user == null || (!user.getUsername().equals(getBean().getUsername()))){
            response.getWriter().write("Y");
        }else{
            response.getWriter().write("N");
        }
        return NONE;
    }

    public String register() throws IOException {
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = getUserService().getUserByUsername(getBean().getUsername());
        if(user != null && user.getUsername().equals(getBean().getUsername())){
            response.getWriter().write("failed:1");
            return NONE;
        }
        getBean().setStatus(0);
        boolean res = getUserService().register(getBean());
        if(res){
            response.getWriter().write("success");
        }else{
            response.getWriter().write("failed:2");
        }
        return NONE;
    }

    public String login() throws IOException {
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = getUserService().getUserByUsername(getBean().getUsername());
        if(user == null || (!user.getPassword().equals(getBean().getPassword()))){
            response.getWriter().write("failed");
        }else{
            ServletActionContext.getRequest().getSession().setAttribute("user", user);
            response.getWriter().write("success");
        }
        return NONE;
    }

    public String changePassword() throws IOException {
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = getUserService().getUserByUsername(getBean().getUsername());
        if(user.getEmail().equals(getBean().getEmail())){
            boolean res = getUserService().changePassword(user, getBean().getPassword());
            if(res){
                response.getWriter().write("success");
            }else{
                response.getWriter().write("failed:4");
            }
        }else{
            response.getWriter().write("failed:3");
        }
        return NONE;
    }

    public String logout() throws IOException {
        HttpSession session = ServletActionContext.getRequest().getSession();
        HttpServletResponse response = ServletActionContext.getResponse();
        session.removeAttribute("user");
        User user = (User) session.getAttribute("user");
        if(user == null){
            response.getWriter().write("success");
        }else{
            response.getWriter().write("failed");
        }
        return NONE;
    }

    public String getStatus() throws IOException {
        HttpServletResponse response = ServletActionContext.getResponse();
        HttpSession session = ServletActionContext.getRequest().getSession();
        User user = (User) session.getAttribute("user");
        if(user == null){
            response.getWriter().write("error");
        }else{
            user = getUserService().getUserById(user.getId());
            response.getWriter().write(String.valueOf(user.getStatus()));
            getUserService().changeStatus(user, 0);
            session.setAttribute("user", user);
        }
        return NONE;
    }

}
