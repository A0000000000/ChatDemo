package com.messageserver.web.action;

import com.messageserver.domain.Message;
import com.messageserver.domain.User;
import org.apache.struts2.ServletActionContext;

import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import java.io.IOException;
import java.util.Date;
import java.util.List;

public class MessageAction extends BaseAction<Message> {

    private String targetId;

    public String sendMessage() throws IOException {
        HttpSession session = ServletActionContext.getRequest().getSession();
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = (User) session.getAttribute("user");
        if(user == null){
            response.getWriter().write("error:1");
        }
        Message message = new Message();
        message.setSenderId(user.getId());
        message.setReceiverId(targetId);
        message.setContent(getBean().getContent());
        message.setStatus(1);
        message.setSendTime(new Date());
        User target = getUserService().getUserById(targetId);
        getUserService().changeStatus(target, target.getStatus() | 2);
        boolean res = getMessageService().addNewMessage(message);
        if(res){
            response.getWriter().write(message.getId()+ "#" + user.getId());
        }else{
            response.getWriter().write("failed");
        }
        return NONE;
    }

    public String getNewMessage() throws IOException {
        HttpSession session = ServletActionContext.getRequest().getSession();
        HttpServletResponse response = ServletActionContext.getResponse();
        User user = (User) session.getAttribute("user");
        if(user == null){
            response.getWriter().write("error");
        }else {
            List<Message> messages = getMessageService().getNewMessageByReceiverId(user.getId());
            StringBuffer sb = new StringBuffer();
            sb.append("[");
            boolean flag = false;
            for (Message message : messages){
                sb.append(message.toJSON());
                sb.append(",");
                flag = true;
                message.setStatus(0);
                getMessageService().saveChange(message);
            }
            if (flag){
                sb.deleteCharAt(sb.length() - 1);
            }
            sb.append("]");
            response.getWriter().write(sb.toString());
        }
        return NONE;
    }

    public String getTargetId() {
        return targetId;
    }

    public void setTargetId(String targetId) {
        this.targetId = targetId;
    }

}
