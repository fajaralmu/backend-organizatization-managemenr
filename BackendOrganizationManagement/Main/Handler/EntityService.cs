﻿using BackendOrganizationManagement.Main.Dto;
using BackendOrganizationManagement.Main.Service;
using BackendOrganizationManagement.Main.Util;
using BackendOrganizationManagement.Models;
using OrgWebMvc.Main.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BackendOrganizationManagement.Main.Handler
{
    public class EntityService
    {
        BaseService baseService;
        private SessionService sessionService = new SessionService();

        public void init()
        { }


        void Debug(string message)
        {
            DebugConsole.Debug(this, message);
        }

        public WebResponse addEntity(WebRequest request, HttpRequest servletRequest, bool newRecord)
        {
            Debug("addEntity: " + request);
            switch (request.entity.ToLower())
            {
                case "user":
                    baseService = new UserService();
                    return saveCommonEntity(request.user, newRecord);

                case "division":
                    baseService = new DivisionService();
                    return saveCommonEntity(request.division, newRecord);

                case "event":
                    baseService = new EventService();
                    return saveCommonEntity(request.Event, newRecord);

                case "position":
                    baseService = new PositionService();
                    return saveCommonEntity(request.position, newRecord);

                case "post":
                    baseService = new PostService();
                    return saveCommonEntity(request.post, newRecord);

                case "program":
                    baseService = new ProgramService();
                    return saveCommonEntity(request.program, newRecord);

                case "section":
                    baseService = new SectionService();
                    return saveCommonEntity(request.section, newRecord);
                case "member":
                    baseService = new MemberService();
                    return saveCommonEntity(request.member, newRecord);

            }

            return WebResponse.failed();
        }

        private WebResponse saveCommonEntity(BaseEntity entity, bool newRecord)
        {
            BaseEntity result = null;

            entity = validateEntity(entity);

            if (newRecord)
            {
                result = baseService.Add(entity);
            }
            else
            {
                result = baseService.Update(entity);
            }

            WebResponse response = WebResponse.success();
            response.entity = entity;
            return response;
        }

        private BaseEntity validateEntity(BaseEntity entity)
        {
            List<PropertyInfo> props = EntityUtil.getDeclaredField(entity.GetType());

            foreach (PropertyInfo prop in props)
            {
                Attribute attr = prop.GetCustomAttribute(typeof(JoinColumn));
                if (attr != null)
                {
                    JoinColumn joinColumn = (JoinColumn)attr;
                    string joinColumnName = joinColumn.Name;

                    object propValue = prop.GetValue(entity);

                    PropertyInfo idFieldOfJoinColumn = EntityUtil.getIdField(propValue.GetType());
                    object id = idFieldOfJoinColumn.GetValue(propValue);

                    PropertyInfo entity_fk = EntityUtil.getSingleDeclaredField(entity.GetType(), joinColumnName);
                    entity_fk.SetValue(entity, id);

                    prop.SetValue(entity, null);
                }
            }
            return entity;
        }

        public WebResponse filter(WebRequest request)
        {
            Type entityClass = null;
            if(request.entity == null)
            {
                return WebResponse.failed();
            }
            switch (request.entity.ToLower())
            {
                case "user":
                    entityClass = typeof(user);
                    this.baseService = new UserService();
                    break;

                case "division":
                    entityClass = typeof(division);
                    this.baseService = new DivisionService();
                    break;

                case "event":
                    entityClass = typeof(@event);
                    this.baseService = new EventService();
                    break;

                case "position":
                    entityClass = typeof(position);
                    this.baseService = new PositionService();
                    break;

                case "post":
                    entityClass = typeof(post);
                    this.baseService = new PostService();
                    break;

                case "program":
                    entityClass = typeof(program);
                    this.baseService = new ProgramService();
                    break;

                case "section":
                    entityClass = typeof(section);
                    this.baseService = new SectionService();
                    break;

                case "member":
                    entityClass = typeof(member);
                    this.baseService = new MemberService();
                    break;
                case "institution":
                    entityClass = typeof(institution);
                    this.baseService = new InstitutionService();
                    break;
                default:
                    return WebResponse.failed();

            }
            Filter filter = request.filter;
            String sql = generateSqlByFilter(filter, entityClass, request);

            int offset = filter.page;// * filter.limit;
            bool withLimit = filter.limit > 0;

            int limit = withLimit ? filter.limit : 0;

            List<BaseEntity> entities = getEntitiesBySql(sql, entityClass, limit, offset);
            int count = baseService.count;

            WebResponse response = WebResponse.success();
            response.entities = entities;
            response.totalData = count;
            return response;
        }

        string[] validateSqlFilter(WebRequest webRequest, Type entityClass, string joinSql, string filterSql)
        {
            SessionData sessionData = sessionService.GetSessionData(webRequest);

            division Division = sessionData.Division;
            if (null == Division)
            {
                return new string[]
                {
                    joinSql, filterSql
                };
            }

            Attribute customAttr = entityClass.GetCustomAttribute(typeof(AdditionalFilter));
            if (null != customAttr)
            {
                AdditionalFilter additionalFilter = (AdditionalFilter)customAttr;

                //join
                if (additionalFilter.join != null)
                {
                    joinSql += " " + additionalFilter.join;

                }
                //filter
                if (additionalFilter.filter != null)

                {
                    const string toReplace = "${filterId}";
                    string replacedParam = additionalFilter.filter.Replace(toReplace, Division.id.ToString());

                    if (filterSql == null || filterSql.Trim().Equals(""))
                    {
                        filterSql = "WHERE " + replacedParam;
                    }
                    else
                    {
                        filterSql += " AND " + replacedParam;
                    }
                }

            }


            return new string[]
              {
                    joinSql, filterSql
              };
        }

        /**
         * 
         * @param filter
         * @param entityClass
         * @return sql & sqlCount
         */
        private string generateSqlByFilter(Filter filter, Type entityClass, WebRequest webRequest)
        {

            //		String entityName = request.entity;
            int offset = filter.page * filter.limit;
            bool withLimit = filter.limit > 0;
            bool withOrder = filter.orderBy != null && filter.orderType != null
                    && !filter.orderBy.Equals("") && !filter.orderType.Equals("");
            bool exacts = filter.exacts;
            bool withFilteredPropertyInfo = null != filter.fieldsFilter && filter.fieldsFilter.Count > 0;

            String orderType = filter.orderType;
            String orderBy = filter.orderBy;
            String tableName = entityClass.Name;

            string orderSQL = withOrder ? generateOrderSql(entityClass, orderType, orderBy) : "";
            String limitOffsetSQL = null;// withLimit ? " LIMIT " + filter.limit + " OFFSET " + offset : "";

            String filterSQL = withFilteredPropertyInfo ? createFilterSQL(entityClass, filter.fieldsFilter, exacts)
                    : "";
            String joinSql = createLeftJoinSQL(entityClass);

            string[] joinFilter = validateSqlFilter(webRequest, entityClass, joinSql, filterSQL);

            String sql = "select  [" + tableName + "].* from [" + tableName + "] " + joinFilter[0] + " " + joinFilter[1] + orderSQL
                    + limitOffsetSQL;
            //String sqlCount = "select COUNT(*) from [" + tableName + "] " + joinSql + " " + filterSQL;
            return sql;
        }

        public List<BaseEntity> getEntitiesBySql(String sql, Type entityClass, int limit, int offset)
        {
            List<object> entities = baseService.SqlList(sql, limit, offset);
            //return EntityUtil.validateDefaultValue(entities);
            List<BaseEntity> result = new List<BaseEntity>();
            foreach (object Object in entities)
            {
                result.Add((BaseEntity)Object);
            }
            return result;
        }

        private static PropertyInfo getPropertyInfoByName(String name, List<PropertyInfo> PropertyInfos)
        {
            foreach (PropertyInfo PropertyInfo in PropertyInfos)
            {
                if (PropertyInfo.Name.Equals(name))
                {
                    return PropertyInfo;
                }
            }
            return null;
        }

        private static String getColumnName(PropertyInfo PropertyInfo)
        {
            if (null == PropertyInfo)
            {
                return null;
            }
            return PropertyInfo.Name;
        }

        private static String createLeftJoinSQL(Type entityClass)
        {
            String tableName = entityClass.Name.Replace("@", "");
            String sql = "";
            List<PropertyInfo> PropertyInfos = EntityUtil.getDeclaredField(entityClass);
            foreach (PropertyInfo PropertyInfo in PropertyInfos)
            {
                JoinColumn joinColumn;
                Attribute Attribute = PropertyInfo.GetCustomAttribute(typeof(JoinColumn));
                if (Attribute != null)
                {
                    joinColumn = (JoinColumn)Attribute;
                    Type PropertyInfoClass = PropertyInfo.PropertyType;
                    string foreignID = joinColumn.Name;
                    string joinTableName = PropertyInfoClass.Name;
                    PropertyInfo idForeignPropertyInfo = EntityUtil.getIdField(PropertyInfoClass);
                    String sqlItem = " LEFT JOIN [$JOIN_TABLE] ON  [$JOIN_TABLE].[$JOIN_ID] = [$ENTITY_TABLE].[$FOREIGN_ID] ";
                    sqlItem = sqlItem.Replace("$FOREIGN_ID", foreignID).Replace("$JOIN_TABLE", joinTableName)
                                            .Replace("$ENTITY_TABLE", tableName).Replace("$JOIN_ID", getColumnName(idForeignPropertyInfo));
                    sql += sqlItem;

                }
            }
            return sql;
        }


        private static String createFilterSQL(Type entityClass, Dictionary<String, Object> filter,
                bool exacts)
        {
            String tableName = entityClass.Name;
            List<String> filters = new List<String>();
            List<PropertyInfo> PropertyInfos = EntityUtil.getDeclaredField(entityClass);

            foreach (String rawKey in filter.Keys)
            {
                String key = rawKey;
                if (filter[rawKey] == null)
                    continue;

                bool itemExacts = exacts;
                bool itemContains = exacts == false;

                if (rawKey.EndsWith("[EXACTS]"))
                {
                    itemExacts = true;
                    itemContains = false;
                    key = rawKey.Split("[EXACTS]".ToCharArray())[0];
                }
                char charz = ',';
                String[] multiKey = key.Split(charz);
                bool isMultiKey = multiKey.Length > 1;
                if (isMultiKey)
                {
                    key = multiKey[0];
                }

                String columnName = key;
                // check if date
                bool dayFilter = key.EndsWith("-day");
                bool monthFilter = key.EndsWith("-month");
                bool yearFilter = key.EndsWith("-year");
                if (dayFilter || monthFilter || yearFilter)
                {
                    String PropertyInfoName = key;
                    String mode = "DAY";
                    String sqlitem = " $MODE([$TABLE_NAME].[$COLUMN_NAME]) = $VALUE ";
                    if (dayFilter)
                    {
                        PropertyInfoName = key.Replace("-day", "");
                        mode = "DAY";

                    }
                    else if (monthFilter)
                    {
                        PropertyInfoName = key.Replace("-month", "");
                        mode = "MONTH";
                    }
                    else if (yearFilter)
                    {
                        PropertyInfoName = key.Replace("-year", "");
                        mode = "YEAR";
                    }
                    PropertyInfo prop = getPropertyInfoByName(PropertyInfoName, PropertyInfos);
                    if (prop == null)
                    {
                        continue;
                    }
                    columnName = prop.Name;
                    sqlitem = sqlitem.Replace("$TABLE_NAME", tableName).Replace("$MODE", mode)
                            .Replace("$COLUMN_NAME", columnName).Replace("$VALUE", filter[key].ToString());
                    filters.Add(sqlitem);
                    continue;
                }

                PropertyInfo prop2 = getPropertyInfoByName(key, PropertyInfos);

                if (prop2 == null)
                {
                    continue;
                }
                if (prop2.GetCustomAttribute(typeof(Column)) != null)

                    columnName = prop2.Name;

                String sqlItem = " [" + tableName + "].[" + columnName + "] ";
                if (prop2.GetCustomAttribute(typeof(JoinColumn)) != null || isMultiKey)
                {
                    Type PropertyInfoClass = prop2.PropertyType;
                    JoinColumn joinCol = (JoinColumn)prop2.GetCustomAttribute(typeof(JoinColumn));
                    String joinTableName = PropertyInfoClass.Name;

                    try
                    {
                        String referencePropertyInfoName = joinCol.Converter;
                        if (isMultiKey)
                        {
                            referencePropertyInfoName = multiKey[1];
                        }
                        PropertyInfo PropertyInfoPropertyInfo = EntityUtil.getSingleDeclaredField(PropertyInfoClass, referencePropertyInfoName);
                        String PropertyInfoColumnName = getColumnName(PropertyInfoPropertyInfo);
                        if (PropertyInfoColumnName == null || PropertyInfoColumnName.Equals(""))
                        {
                            PropertyInfoColumnName = key;
                        }
                        sqlItem = " [" + joinTableName + "].[" + PropertyInfoColumnName + "] ";
                    }
                    catch (Exception e)
                    {

                        continue;
                    }

                }
                // rollback key to original key
                /*
                 * if (isMultiKey) { key = String.join(",", multiKey); if
                 * (rawKey.EndsWith("[EXACTS]")) { key+="[EXACTS]"; } }
                 */
                if (itemContains)
                {
                    sqlItem += " LIKE '%" + filter[rawKey] + "%' ";
                }
                else if (itemExacts)
                {
                    sqlItem += " = '" + filter[rawKey] + "' ";
                }

                filters.Add(sqlItem);
            }
            if (filters == null || filters.Count == 0)
            {
                return "";
            }
            return " WHERE " + String.Join(" AND ", filters);
        }



        private static String generateOrderSql(Type entityClass, String orderType, String orderBy)
        {
            // set order by 
            PropertyInfo orderByPropertyInfo = EntityUtil.getSingleDeclaredField(entityClass, orderBy);
            if (orderByPropertyInfo == null)
            {
                return null;
            }

            String columnName = orderBy;
            String tableName = entityClass.Name;

            Attribute joinColumnAttr = orderByPropertyInfo.GetCustomAttribute(typeof(JoinColumn));

            if (joinColumnAttr != null)
            {
                Type PropertyInfoClass = orderByPropertyInfo.PropertyType;
                tableName = (PropertyInfoClass).Name;

                try
                {
                    JoinColumn formPropertyInfo = (JoinColumn)joinColumnAttr;
                    PropertyInfo PropertyInfoPropertyInfo = PropertyInfoClass.GetProperty(formPropertyInfo.Converter);
                    columnName = formPropertyInfo.Converter;// getColumnName(PropertyInfoPropertyInfo);
                    if (null == columnName)
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else {
                columnName = orderBy;
            }

            String orderPropertyInfo = "[" + tableName + "].[" + columnName + "]";
            return " ORDER BY " + orderPropertyInfo + " " + orderType;
        }

        private static String getTableName(Type entityClass)
        {
            return entityClass.Name;
        }

        public WebResponse delete(WebRequest request)
        {
            Dictionary<String, Object> filter = request.filter.fieldsFilter;
            try
            {
                switch (request.entity)
                {
                    case "user":
                        new UserService().Delete(request.user);
                        break;

                    case "division":
                        new DivisionService().Delete(request.division);
                        break;

                    case "event":
                        new EventService().Delete(request.Event);
                        break;

                    case "position":
                        new PositionService().Delete(request.position);
                        break;

                    case "post":
                        new PostService().Delete(request.post);
                        break;

                    case "program":
                        new ProgramService().Delete(request.program);
                        break;

                    case "section":
                        new SectionService().Delete(request.section);
                        break;

                    case "member":
                        new MemberService().Delete(request.member);
                        break;
                    default:
                        return WebResponse.failed();
                }
                return WebResponse.success();
            }
            catch (Exception ex)
            {
                ex.ToString();
                return WebResponse.failed();
            }
        }
    }
}