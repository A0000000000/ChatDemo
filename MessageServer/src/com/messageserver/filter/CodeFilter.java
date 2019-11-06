package com.messageserver.filter;

import javax.servlet.*;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.lang.reflect.Proxy;
import java.util.HashMap;
import java.util.Map;

public class CodeFilter implements Filter {
    public void destroy() {
    }

    public void doFilter(ServletRequest req, ServletResponse resp, FilterChain chain) throws ServletException, IOException {
        final HttpServletRequest request = (HttpServletRequest) req;
        HttpServletResponse response = (HttpServletResponse) resp;
        if(request.getRequestURI().endsWith(".css") || request.getRequestURI().endsWith(".js")){
            chain.doFilter(req, resp);
            return;
        }
        String type = request.getMethod();
        response.setContentType("text/html;charset=UTF-8");
        if("POST".equalsIgnoreCase(type)){
            request.setCharacterEncoding("UTF-8");
            chain.doFilter(request, response);
        }else{
            ServletRequest requestProxy = (ServletRequest) Proxy.newProxyInstance(request.getClass().getClassLoader(),request.getClass().getInterfaces(),
                    (proxy, method, args) -> {
                        if(method.getName().equalsIgnoreCase("getParameterMap")){
                            Map<String, String[]> map = (Map<String, String[]>) method.invoke(request, args);
                            Map<String, String[]> ret = new HashMap<>();
                            for (Map.Entry<String, String[]> item : map.entrySet()){
                                String[] values = new String[item.getValue().length];
                                for(int i = 0; i < values.length; i++){
                                    values[i] = new String(item.getValue()[i].getBytes("ISO-8859-1"), "UTF-8");
                                }
                                ret.put(item.getKey(), values);
                            }
                            return ret;
                        }else if(method.getName().equalsIgnoreCase("getParameterValues")){
                            String[] strings = (String[]) method.invoke(request, args);
                            String[] ret = new String[strings.length];
                            for (int i = 0; i < strings.length; i++){
                                ret[i] = new String(strings[i].getBytes("ISO-8859-1"), "UTF-8");
                            }
                            return ret;

                        }else if(method.getName().equalsIgnoreCase("getParameter")){
                            String str = (String) method.invoke(request, args);
                            String ret = new String(str.getBytes("ISO-8859-1"), "UTF-8");
                            return ret;
                        }else{
                            return method.invoke(request, args);
                        }
                    });
            chain.doFilter(requestProxy, response);
        }
    }

    public void init(FilterConfig config) throws ServletException {

    }

}
