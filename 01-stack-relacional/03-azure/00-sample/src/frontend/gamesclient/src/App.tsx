import { BrowserRouter } from 'react-router-dom';
import './App.css';
import { Template } from './components/template/template';
import { DefaultRouter } from './routes/routes';

function App() {
  return (
    <BrowserRouter>
      <DefaultRouter />
      <div className="App">
      </div>
    </BrowserRouter>
  );
}

export default App;
