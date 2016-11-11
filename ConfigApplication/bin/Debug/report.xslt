<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt">
  <xsl:template match="Report">
    <html>
      <head></head>
      <body>
        <H2>
          <xsl:element name="a">
            <xsl:attribute name="href">
              <xsl:value-of select="OrganizationUrl"/>
            </xsl:attribute>
            Организация <xsl:value-of select="OrganizationName"/>
          </xsl:element>
        </H2>
        <p>
          Всего карточек:
          <xsl:element name="b">
            <xsl:value-of select="CardsCount"/>
          </xsl:element>
        </p>
        <p>
          Всего выполненных карточек:
          <xsl:element name="b">
            <xsl:value-of select="CompletedCardsCount"/>
          </xsl:element>
        </p>
        <p>
          Всего просроченных карточек:
          <xsl:element name="b">
            <xsl:value-of select="OverdueCardsCount"/>
          </xsl:element>
        </p>
        <h3>Детализация по просроченным карточкам</h3>
        <xsl:for-each select="OverdueCardsMembers//Member">
          <p>
            <xsl:element name="b">
              <xsl:value-of select="FullName"/>
            </xsl:element>
            <ul>
              <xsl:for-each select="MemembersOverdueCards//CardRepresent">
                <li>
                  <xsl:element name="b">
                    <xsl:value-of select="DueDateRepresent"/>
                  </xsl:element>
                  <br/>
                  <xsl:element name="span">
                    <xsl:value-of select="Text"/>
                  </xsl:element>
                  <br/>
                  <xsl:element name="a">
                    <xsl:attribute name="href">
                      <xsl:value-of select="CardUrl"/>
                    </xsl:attribute>
                    <xsl:value-of select="CardUrl"/>
                  </xsl:element>
                </li>
              </xsl:for-each>
            </ul>
          </p>
        </xsl:for-each>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>