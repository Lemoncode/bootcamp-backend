import { useTheme } from 'react-jss';
import { NavLink } from 'react-router-dom';
import i18n from '../../../../services/i18n';
import { INavItem } from '../../../../models/sitenav/INavItem';
import { SiteNavItemStyles } from './siteNavItem.jss';

export interface ISiteNavItemProps {
    navItem: INavItem;
}

export const SiteNavItem = (props: ISiteNavItemProps) => {
    const theme = useTheme();
    const styles = SiteNavItemStyles({ theme });

    return (
        <>
            <NavLink
                className={({ isActive }) => (isActive ? styles.itemActive : styles.item)}
                to={props.navItem.link}
                target={props.navItem.target}
                replace={true}>
                    {i18n.t(props.navItem.title)}
            </NavLink>
        </>
    );
};
