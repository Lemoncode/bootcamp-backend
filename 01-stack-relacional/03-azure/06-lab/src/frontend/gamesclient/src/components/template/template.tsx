import React, { useEffect } from "react";
import { ThemeProvider } from "react-jss";
import { defaultTheme } from "../../theme/themes";
import { SiteHeader } from "./siteHeader/siteHeader";
// import { SiteNav } from "./siteNav/siteNav";

interface TemplateProps {
    featureId: string;
    component: React.ReactNode | React.Component | React.FunctionComponent;
}

export const Template = (props: TemplateProps & any) => {
    const [currentTheme, setCurrentTheme] = React.useState(defaultTheme);

    // function changeTheme(themeValue: string) {
    //     if (themeValue === "RETROGAMES-THEME")
    //         setCurrentTheme(defaultTheme);
    //     else
    //         setCurrentTheme(defaultTheme);
    // }

    useEffect(() => {
        setCurrentTheme(defaultTheme);
    }, [])

    return (
        <>
            <ThemeProvider theme={currentTheme}>
                <BodyContainer
                    component={props.component}
                    changeTheme={currentTheme}
                    props={props}
                />
            </ThemeProvider>

        </>
    )
}

export const BodyContainer = (props: any) => {
    const content = props.component ? <props.component {...props.props} /> : <p>Template referenced without content component</p>;

    return (
        <>
            <div className="">
                <div className="">
                    <div>
                        {/* <SiteNav menuCollapsed={false} /> */}
                    </div>
                </div>
                <div className="">
                    <SiteHeader />
                    <div className="">{content}</div>
                </div>
            </div>
        </>
    )
};