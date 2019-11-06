package com.messageserver.web.action;

import com.messageserver.domain.User;
import com.messageserver.service.UserService;
import com.messageserver.utils.ServiceFactory;
import com.messageserver.utils.SessionThreadLocal;
import com.opensymphony.xwork2.ActionSupport;
import com.opensymphony.xwork2.ModelDriven;
import org.apache.struts2.ServletActionContext;

import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import java.io.IOException;

public class UserAction extends ActionSupport implements ModelDriven<User> {

    private User obj = new User();

    private UserService userService = (UserService) ServiceFactory.newServiceInstance("UserService");

    public String checkUsername() throws IOException {
        SessionThreadLocal.openSession();
        User user = userService.getUserByUsername(obj.getUsername());
        HttpServletResponse response = ServletActionContext.getResponse();
        if(user == null || (!user.getUsername().equals(obj.getUsername()))){
            response.getWriter().write("Y");
        }else{
            response.getWriter().write("N");
        }
        SessionThreadLocal.closeSession();
        return NONE;
    }

    public String register() throws IOException {
        SessionThreadLocal.openSession();
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = userService.getUserByUsername(obj.getUsername());
        if(user != null && user.getUsername().equals(obj.getUsername())){
            response.getWriter().write("failed:1");
            SessionThreadLocal.closeSession();
            return NONE;
        }
        obj.setStatus(0);
        boolean res = userService.register(obj);
        if(res){
            response.getWriter().write("success");
        }else{
            response.getWriter().write("failed:2");
        }
        SessionThreadLocal.closeSession();
        return NONE;
    }

    public String login() throws IOException {
        SessionThreadLocal.openSession();
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = userService.getUserByUsername(obj.getUsername());
        if(user == null || (!user.getPassword().equals(obj.getPassword()))){
            response.getWriter().write("failed");
        }else{
            ServletActionContext.getRequest().getSession().setAttribute("user", user);
            response.getWriter().write("success");
        }
        SessionThreadLocal.closeSession();
        return NONE;
    }

    public String changePassword() throws IOException {
        SessionThreadLocal.openSession();
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = userService.getUserByUsername(obj.getUsername());
        if(user.getEmail().equals(obj.getEmail())){
            boolean res = userService.changePassword(user, obj.getPassword());
            if(res){
                response.getWriter().write("success");
            }else{
                response.getWriter().write("failed:4");
            }
        }else{
            response.getWriter().write("failed:3");
        }
        SessionThreadLocal.closeSession();
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
        SessionThreadLocal.openSession();
        HttpServletResponse response = ServletActionContext.getResponse();
        HttpSession session = ServletActionContext.getRequest().getSession();
        User user = (User) session.getAttribute("user");
        if(user == null){
            response.getWriter().write("error");
        }else{
            user = userService.getUserById(user.getId());
            response.getWriter().write(String.valueOf(user.getStatus()));
            userService.changeStatus(user, 0);
            session.setAttribute("user", user);
        }
        SessionThreadLocal.closeSession();
        return NONE;
    }

    public User getObj() {
        return obj;
    }

    public void setObj(User obj) {
        this.obj = obj;
    }

    @Override
    public User getModel() {
        return obj;
    }
}
